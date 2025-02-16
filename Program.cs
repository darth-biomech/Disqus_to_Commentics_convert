using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DB_Disqus_convert
{
    static class Program
    {
        public static MainBody thisProgram;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            thisProgram = new MainBody();
            thisProgram.Initialize();
            Application.Run(new Form1());
        }

        public static void SetInputFile(string file)
        {
            thisProgram.inputFile = file;
        }

        public static void SetOutputFile(string file)
        {
            thisProgram.pagesLookupFile = file;
        }

        public static void Begin()
        {
            thisProgram.Start();
        }
    }
    public class Comment
    {
        public int id = 0;
        public int pageId = 0;
        public string commentID = string.Empty;
        public string parentID = string.Empty;
        public int authorID = -1;
        public string text = string.Empty;
        public string date = string.Empty;
    }
    public class CommentThread
    {
        public int id = 0;
        public int siteId = 0;
        public string identifier = string.Empty;
        public string reference = string.Empty;
        public string url = string.Empty;
        public string moderate = string.Empty;
        public string isFormEnabled = string.Empty;
        public string DateCreated = string.Empty;
        public string DateModified = string.Empty;
    }
    public class PageDB
    {
        public string Id = string.Empty;
        public string Title = string.Empty;
        public string BookId = string.Empty;
        public string MarkerId = string.Empty;
        public string ToneId = string.Empty;
        public string SortOrder = string.Empty;
        public string DateCreated = string.Empty;
        public string DateModified  = string.Empty;
        public string DatePublish = string.Empty;
        public string Options = string.Empty;
        public string Description = string.Empty;
    }
    class MainBody
    {
        private IniFile _settingsIni = new IniFile();

        public void Initialize()
        {
            LoadSettings();
        }
        
        public Form1 gui;
        public string inputFile;
        public string pagesLookupFile;
        public string defaultEmail = "noreply@yoursite.com";
        public int numOfComments = 0;
        public int numOfUsers = 1;
        public bool skipDeletedPosts = false;

        

        
        public void SaveSettings()
        {
            _settingsIni.Write("inputFile", inputFile, "TransferSettings");
            _settingsIni.Write("outputFile", pagesLookupFile, "TransferSettings");
            _settingsIni.Write("defaultEmail", defaultEmail, "TransferSettings");
            _settingsIni.Write("numOfComments", numOfComments.ToString(), "TransferSettings");
            _settingsIni.Write("numOfUsers", numOfUsers.ToString(), "TransferSettings");
            _settingsIni.Write("skipDeletedPosts", skipDeletedPosts.ToString(), "TransferSettings");
            
        }
        public void LoadSettings()
        {
            if (inputFile == String.Empty)
                inputFile = Application.StartupPath;
            TryGetSettingValue("inputFile", ref inputFile);
            TryGetSettingValue("outputFile", ref pagesLookupFile);
            TryGetSettingValue("defaultEmail", ref defaultEmail);
            TryGetSettingValue("numOfComments", ref numOfComments);
            TryGetSettingValue("numOfUsers", ref numOfUsers);
            TryGetSettingValue("skipDeletedPosts", ref skipDeletedPosts);
        }
 
        private void TryGetSettingValue(string key, ref int var)
        {
            if (!_settingsIni.KeyExists(key, "TransferSettings"))
            {
                _settingsIni.Write(key, var.ToString(), "TransferSettings");
            }
            else
            {
                var = Int32.Parse(_settingsIni.Read(key, "TransferSettings"));
            }
        }
        private void TryGetSettingValue(string key, ref bool var)
        {
            if (!_settingsIni.KeyExists(key, "TransferSettings"))
            {
                _settingsIni.Write(key, var.ToString(), "TransferSettings");
            }
            else
            {
                var = _settingsIni.Read(key, "TransferSettings").ToLower() == "true";
            }
        }
        private void TryGetSettingValue(string key, ref string var)
        {
            if (!_settingsIni.KeyExists(key, "TransferSettings"))
            {
                _settingsIni.Write(key, var, "TransferSettings");
            }
            else
            {
                var = _settingsIni.Read(key, "TransferSettings");
            }
        }
        
        public Dictionary<string, int> threadMap = new Dictionary<string, int>(); // discus ID vs commentics ID
        public Dictionary<string, int> usersMap = new Dictionary<string, int>(); // discus name vs commentics ID
        public Dictionary<string, int> commentsMap = new Dictionary<string, int>(); // discus ID vs commentics ID

        public List<CommenticsPageEntry> pageDb = new List<CommenticsPageEntry>();
        
        DiDB disqusDB;

        private int steps = 6;
        private int currentStep = 0;
        public void IncrementProgress()
        {
            currentStep++;
            gui.progressBar.Value = (int) Math.Round(((float) currentStep / steps) * 100);
        }
        public void Start()
        {
            gui.progressBar.Value = 0;
            currentStep = 0;
            if (StartProcessing())
            {
                gui.Log("--------------------------------------------------");
                gui.Log(" ");
                gui.Log("Conversion finished!");
            }
            else
            {
                gui.Log("--------------------------------------------------");
                gui.Log(" ");
                gui.Log("Conversion had errors!");
            }
            ExportFileString("ConversionLog",gui.GetLog(), ".log");
        }
        public bool StartProcessing()
        {
            if (inputFile != String.Empty && pagesLookupFile != String.Empty)
            {
                gui.Log("Starting conversion...");

                threadMap.Clear();
                usersMap.Clear();
                commentsMap.Clear();
                pageDb.Clear();
                disqusDB = null;
                
                if (File.Exists(inputFile))
                {
                    disqusDB = DiDB.Create(File.ReadAllText(inputFile));
                    if(disqusDB == null)
                    {
                        gui.Log("Couldn't create DB");
                        return false;
                    }
                    gui.Log("Disqus backup parsed!");
                    gui.Log("Got "+disqusDB.categories.Count+" categories");
                    gui.Log("Got "+disqusDB.users.Count+" users");
                    gui.Log("Got "+disqusDB.threads.Count+" threads");
                    gui.Log("Got "+disqusDB.posts.Count+" posts");
                    IncrementProgress();
                }
                else
                {
                    gui.Log("Disqus backup file not found!");
                    return false;
                }
                
                if (File.Exists(pagesLookupFile))
                {
                    gui.Log("Reading pages lookup file...");
                    string pagesData = File.ReadAllText(pagesLookupFile);
                    if (pagesData.IndexOf("CREATE TABLE "+"`pages`", StringComparison.Ordinal) == -1)
                    {
                        gui.Log("Pages lookup file is not an SQL backup!");
                        return false;
                    }
                    List<string> allPages = new();
                    string[] allInserts = pagesData.Split(new []{"INSERT INTO `pages`"}, StringSplitOptions.RemoveEmptyEntries);
                    for (int index = 0; index < allInserts.Length; index++)
                    {
                        string s = allInserts[index];
                        if(s.IndexOf("VALUES", StringComparison.Ordinal) == -1) continue;
                        allInserts[index] = s.Substring(s.IndexOf("VALUES", StringComparison.Ordinal)).Split(new []{"'');"},StringSplitOptions.None)[0]+"'')";
                        allPages.AddRange(allInserts[index].Split(Convert.ToChar('\n')));
                    }

                    pageDb.Clear();
                    int succPages = 0;
                    for (int i = 1; i < allPages.Count; i++)
                    {
                        if(CommenticsPageEntry.TryCreateEntry(allPages[i].Trim().TrimStart('(').TrimEnd(',').TrimEnd(')'),out CommenticsPageEntry entry))
                        {
                            pageDb.Add(entry);
                            succPages++;
                        }
                    }
                    gui.Log("Read "+pageDb.Count+" pages");
                    if(pageDb.Count > succPages)
                        gui.Log("Failed to parse "+(pageDb.Count-succPages)+" pages");
                    IncrementProgress();
                }
                else
                {
                    gui.Log("Pages lookup file not found!");
                    return false;
                }
                
                if (pageDb.Count > 0 && disqusDB.threads.Count > 0)
                {
                    List<string> orphanURLs = new();
                    foreach (DiThread diThread in disqusDB.threads)
                    {
                        bool found = false;
                        foreach (CommenticsPageEntry pageEntry in pageDb)
                        {
                            if(!threadMap.ContainsKey(diThread.threadId))
                                if (pageEntry.url != null && diThread.link != null && pageEntry.url.Contains(diThread.link))
                                {
                                    threadMap.Add(diThread.threadId, int.Parse(pageEntry.identifier));
                                    found = true;
                                    break;
                                }
                        }
                        if(!found)
                            orphanURLs.Add(diThread.link);
                    }
                    if(orphanURLs.Count > 0)
                    {
                        gui.Log("Couldn't find "+orphanURLs.Count+" URLs in pages lookup file:",false);
                        foreach (string orphanUrl in orphanURLs)
                        {
                            if(orphanUrl != null)
                                gui.Log("\t"+orphanUrl,false);
                        }
                        gui.Log("",false);
                        gui.Log("Try to visit every page on your site that has Commentics form so that it would " + Environment.NewLine +
                                "add those URL entries in its database, re-Export Pages table SQL backup, and try again");
                    }
                    else
                    {
                        gui.Log("All URLs were located!");
                    }
                    IncrementProgress();
                }

                for (int i = 0; i < disqusDB.posts.Count; i++)
                    if (disqusDB.posts[i].commentId != null && disqusDB.posts[i].commentId != "")
                        commentsMap.Add(disqusDB.posts[i].commentId, i);
                for (int i = 0; i < disqusDB.users.Count; i++)
                    if (disqusDB.users[i].username != null && disqusDB.users[i].username != "")
                        usersMap.Add(disqusDB.users[i].username, i);
                IncrementProgress();

                List<string> outUsers = new();
                
                int countLines = 0;
                for (int i = 0; i < disqusDB.users.Count; i++)
                {
                    countLines++;
                    if(countLines > DiDB.tablelineSeparator)
                    {
                        countLines = 0;
                        outUsers[outUsers.Count-1] = outUsers[outUsers.Count-1].TrimEnd().TrimEnd(',', ' ')+";";
                        outUsers.Add(DiDB.insertUsersTableLine);
                    }
                    DiAuthor user = disqusDB.users[i];
                    outUsers.Add(user.FormatEntry(i+numOfUsers, defaultEmail, i == disqusDB.users.Count - 1));
                }
                string finUserFile = DiDB.startOfUsersFile + DiDB.insertUsersTableLine;
                foreach (string user in outUsers)
                {
                    finUserFile += user;
                }
                finUserFile += DiDB.endOfUsersFile;
                ExportFileString("Users_DB",finUserFile);
                IncrementProgress();
                countLines = 0;
                List<string> outComments = new();
                for (int i = 0; i < disqusDB.posts.Count; i++)
                {
                    DiPost post = disqusDB.posts[i];
                    if(post.isDeleted == "true" && skipDeletedPosts)continue;
                    
                    countLines++;
                    if(countLines > DiDB.tablelineSeparator)
                    {
                        countLines = 0;
                        outComments[outComments.Count-1] = outComments[outComments.Count-1].TrimEnd().TrimEnd(',', ' ')+";";
                        outComments.Add(DiDB.insertCommentsTableLine);
                    }
                    
                    CommenticsCommentEntry comment = new CommenticsCommentEntry();
                    comment.Init();
                    comment.id = i+numOfComments;
                    comment.is_verified = post.isSpam == "true" ? 0 : 1;
                    comment.is_approved = comment.is_verified;
                    comment.is_locked = post.isDeleted == "true" ? 1 : 0;
                    
                    int users;
                    if (usersMap.ContainsKey(post.author))
                        users = usersMap[post.author];
                    else
                        users = usersMap.Count + i;

                    comment.user_id = users+numOfUsers;
                    if (threadMap.ContainsKey(post.threadId))
                        comment.page_id = threadMap[post.threadId];
                    else
                        comment.page_id = post.threadId.GetHashCode()+i;
                    comment.comment = post.message.Replace("\'", "&#39;");
                    comment.date_added = post.createdAt;
                    comment.date_modified = post.createdAt;
                    comment.reply_to = commentsMap.ContainsKey(post.parentId)?commentsMap[post.parentId]+numOfComments:0;
                    outComments.Add(comment.FormatEntry(i == disqusDB.posts.Count - 1));
                }
                string finCommentsFile = DiDB.startOfCommentsFile + DiDB.insertCommentsTableLine;
                foreach (string outComment in outComments)
                {
                    finCommentsFile += outComment;
                }
                finCommentsFile += DiDB.endOfCommentsFile;
                ExportFileString("Comments_DB",finCommentsFile);
                IncrementProgress();
                return true;
            }

            return false;
        }

        public void ExportFileString(string filename, string content, string ext = ".sql")
        {
            string filepath = Path.GetDirectoryName(pagesLookupFile)+"/"+filename+ext;
            
            File.Delete(filepath);
            File.WriteAllText(filepath, content);
            gui.Log("Saving '"+filename+ext+"' into output directory");
        }
    }

    public class DiDB
    {
        public static DiDB Create(string content)
        {
            DiDB newDB = new DiDB();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content.Replace("dsq:id", "dsq_id"));
            XmlNodeList categories = doc.GetElementsByTagName("category");
            foreach (XmlNode category in categories)
            {
                DiCategory newCat = new DiCategory();
                if (category.Attributes != null)
                    newCat.categoryId = category.Attributes["dsq_id"].Value;
                newCat.forum = category["forum"]?.InnerText;
                newCat.title = category["title"]?.InnerText;
                newCat.isDefault = category["isDefault"]?.InnerText == "true";
                newDB.categories.Add(newCat);
            }
            XmlNodeList threads = doc.GetElementsByTagName("thread");
            foreach (XmlNode thread in threads)
            {
                DiThread newThread = new DiThread();
                newThread.Init();
                newThread.author = GrabUser(thread, newDB,true);
                if (thread.Attributes != null) 
                    newThread.threadId = thread.Attributes["dsq_id"].Value;
                newThread.id = thread["id"]?.InnerText;
                newThread.forum = thread["forum"]?.InnerText;
                newThread.category = thread["category"]?.InnerText;
                newThread.link = thread["link"]?.InnerText.Split('&')[0].Split('?')[0].Split('#')[0].Replace("http://","").Replace("https://", "").Replace("www.", "");
                newThread.title = thread["title"]?.InnerText;
                newThread.message = thread["message"]?.InnerText;
                newThread.createdAt = thread["createdAt"] != null?DiDB.FormatDate(thread["createdAt"].InnerText):DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                newThread.isClosed = thread["isClosed"]?.InnerText;
                newThread.isDeleted = thread["isDeleted"]?.InnerText;
                newDB.threads.Add(newThread);
            }
            XmlNodeList posts = doc.GetElementsByTagName("post");
            foreach (XmlNode post in posts)
            {
                if(post == null) continue;
                DiPost newPost = new DiPost();
                newPost.Init();
                newPost.author = GrabUser(post, newDB);
                if (post.Attributes != null) 
                    newPost.commentId = post.Attributes["dsq_id"].Value;
                newPost.message = post["message"]?.InnerText.Replace("<![CDATA[", "").Replace("]]>", "");
                newPost.createdAt = post["createdAt"] != null?DiDB.FormatDate(post["createdAt"].InnerText):DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                newPost.isDeleted = post["isDeleted"]?.InnerText;
                newPost.isSpam = post["isSpam"]?.InnerText;
                if(post["thread"] != null)
                {
                    newPost.threadId = post["thread"].Attributes["dsq_id"].Value;
                }
                if(post["parent"] != null)
                    newPost.parentId = post["parent"].Attributes["dsq_id"].Value;
                newDB.posts.Add(newPost);
            }
            
            return newDB;
        }
        private static string GrabUser(XmlNode source, DiDB newDB, bool admin = false)
        {
            XmlNodeList content = source.SelectNodes("*");
            XmlNode author = source["author"]; 
            if (author == null) return "";
            
            if(newDB.users.Exists(x => x.username == author["username"]?.InnerText))
                return author["username"]?.InnerText??"anonymous";
            
            DiAuthor newUser = new DiAuthor();
            newUser.name = author["name"]?.InnerText;
            newUser.username = author["username"]?.InnerText;
            newUser.isAnonymous = author["isAnonymous"]?.InnerText == "true";
            if (newUser.isAnonymous)
            {
                newUser.username = "anonymous";
                newUser.name = "Anonymous";
            }
            newUser.isAdmin = admin;
            newUser.time = source["createdAt"] != null?DiDB.FormatDate(source["createdAt"].InnerText):DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            newDB.users.Add(newUser);
            
            return newUser.username;
            
        }

        public List<DiCategory> categories = new List<DiCategory>();
        public List<DiAuthor> users = new List<DiAuthor>();
        public List<DiThread> threads = new List<DiThread>();
        public List<DiPost> posts = new List<DiPost>();
        public static string FormatDate(string inp)
        {
            string date = inp;
            //2021-08-24T17:34:28Z
            //2022-11-01 06:41:14
            date = date.Replace("Z", "");
            date = date.Replace("T", " ");
            return date;
        }
        #region SQL file templates

        public static int tablelineSeparator = 96;
        public static string startOfCommentsFile = "-- phpMyAdmin SQL Dump\n-- version 5.2.1\n-- https://www.phpmyadmin.net/\n--\n\n--\n-- Dumping data for table `comments`\n--\n\n";
        public static string insertCommentsTableLine = "\nINSERT INTO `comments` (`id`, `user_id`, `page_id`, `website`, `town`, `state_id`, `country_id`, `rating`, `reply_to`, `headline`, `comment`, `reply`, `ip_address`, `is_approved`, `notes`, `is_admin`, `is_sent`, `sent_to`, `likes`, `dislikes`, `reports`, `is_sticky`, `is_locked`, `is_verified`, `date_modified`, `date_added`) VALUES\n";
        public static string endOfCommentsFile = "\n--\n-- Indexes for dumped tables\n--\n\n--\n-- Indexes for table `comments`\n--\nALTER TABLE `comments`\n  ADD PRIMARY KEY (`id`);\n\n--\n-- AUTO_INCREMENT for dumped tables\n--\n\n--\n-- AUTO_INCREMENT for table `comments`\n--\nALTER TABLE `comments`\n  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2871;\nCOMMIT;\n\n/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;\n/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;\n/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;\n";
        
        public static string startOfUsersFile = "-- phpMyAdmin SQL Dump\n-- version 5.2.1\n-- https://www.phpmyadmin.net/\n\nSET SQL_MODE = \"NO_AUTO_VALUE_ON_ZERO\";\nSTART TRANSACTION;\nSET time_zone = \"+00:00\";\n\n\n/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;\n/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;\n/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;\n/*!40101 SET NAMES utf8mb4 */;\n\n--\n-- Dumping data for table `users`\n--\n\n";
        public static string insertUsersTableLine = "\nINSERT INTO `users` (`id`, `avatar_id`, `avatar_pending_id`, `avatar_selected`, `avatar_login`, `name`, `email`, `moderate`, `token`, `to_all`, `to_admin`, `to_reply`, `to_approve`, `format`, `ip_address`, `date_modified`, `date_added`) VALUES\n";
        public static string endOfUsersFile = "\n--\n-- Indexes for dumped tables\n--\n\n--\n-- Indexes for table `users`\n--\nALTER TABLE `users`\n  ADD PRIMARY KEY (`id`);\n\n--\n-- AUTO_INCREMENT for dumped tables\n--\n\n--\n-- AUTO_INCREMENT for table `users`\n--\nALTER TABLE `users`\n  MODIFY `id` int UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=272;\nCOMMIT;\n\n/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;\n/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;\n/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;\n";
    

        #endregion
    }
    public struct DiCategory
    {
        public string categoryId;
        public string forum, title;
        public bool isDefault;
    }
    public struct DiAuthor
    {
        public string name, username, time;
        public bool isAnonymous,isAdmin;

        public string FormatEntry(int id,string email="noreply@google.com",bool last = false)
        {
            string result = "("+id+", 0, 0, '', '', '"+name+"', '"+email+"', 'default', '"+Guid.NewGuid()+"', 1, 1, 1, 1, 'html', '127.0.0.1', '"+time+"', '"+time+"')";
            result += (last?";":",")+Environment.NewLine;
            return result;
        }
    }
    public struct DiThread
    {
        public string threadId;
        public string id;
        public string forum;
        public string category;
        public string link;
        public string title;
        public string message;
        public string createdAt;
        public string author;
        public string isClosed;
        public string isDeleted;
        public void Init()
        {
            threadId = string.Empty;
            id = string.Empty;
            forum = string.Empty;
            category = string.Empty;
            link = string.Empty;
            title = string.Empty;
            message = string.Empty;
            createdAt = string.Empty;
            author = string.Empty;
            isClosed = string.Empty;
            isDeleted = string.Empty;
        }
    }
    public struct DiPost
    {
        public string commentId;
        public string message;
        public string createdAt;
        public string isDeleted;
        public string isSpam;
        public string author;
        public string threadId;
        public string parentId;

        public void Init()
        {
            commentId = string.Empty;
            message = string.Empty;
            createdAt = string.Empty;
            isDeleted = string.Empty;
            isSpam = string.Empty;
            author = "anonimous";
            threadId = "-1";
            parentId = "-1";
        }
    }

    public struct CommenticsPageEntry
    {
        public int id;
        public int site_id;
        public string identifier;
        public string reference;
        public string url;
        public string moderate;
        public int is_form_enabled;
        public string date_modified;
        public string date_added;
        
        public static bool TryCreateEntry(string sourceLine, out CommenticsPageEntry newItem)
        {
            newItem = new();
            string[] parts = sourceLine.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i]=parts[i].Trim().Trim('\'');
            }

            if(parts.Length < 9) return false;
            if(int.TryParse(parts[0], out int id))
                newItem.id = id;
            if(int.TryParse(parts[1], out int siteid))
                newItem.site_id = siteid;
            newItem.identifier = parts[2];
            newItem.reference = parts[3];
            newItem.url = parts[4];
            newItem.moderate = parts[5];
            if(int.TryParse(parts[6], out int form))
                newItem.is_form_enabled = form;
            newItem.date_modified = parts[7];
            newItem.date_added = parts[8];
            return true; 
        }
    }
    public struct CommenticsCommentEntry
    {
        public int id;
        public int user_id;
        public int page_id;
        public string website;
        public string town;
        public int state_id;
        public int country_id;
        public int rating;
        public int reply_to;
        public string headline;
        public string comment;
        public string reply;
        public string ip_address;
        public int is_approved;
        public string notes;
        public int is_admin;
        public int is_sent;
        public int sent_to;
        public int likes;
        public int dislikes;
        public int reports;
        public int is_sticky;
        public int is_locked;
        public int is_verified;
        public string date_modified;
        public string date_added;

        public void Init()
        {
            website = string.Empty;
            town = string.Empty;
            headline = string.Empty;
            comment = string.Empty;
            reply = string.Empty;
            ip_address = "127.0.0.1";
            is_approved = 1;
            notes = "Transfered from Disqus";
            is_sent = 1;
            is_verified = 1;
            date_modified = string.Empty;
            date_added = string.Empty;
        }
        public string FormatEntry(bool last = false)
        {
            string result = "(";
            result += id.ToString() + ", ";
            result += user_id.ToString() + ", ";
            result += page_id.ToString() + ", ";
            result += "'" + website + "', ";
            result += "'" + town + "', ";
            result += state_id.ToString() + ", ";
            result += country_id.ToString() + ", ";
            result += rating.ToString() + ", ";
            result += reply_to.ToString() + ", ";
            result += "'" + headline + "', ";
            result += "'" + comment + "', ";
            result += "'" + reply + "', ";
            result += "'" + ip_address + "', ";
            result += is_approved.ToString() + ", ";
            result += "'" + notes + "', ";
            result += is_admin.ToString() + ", ";
            result += is_sent.ToString() + ", ";
            result += sent_to.ToString() + ", ";
            result += likes.ToString() + ", ";
            result += dislikes.ToString() + ", ";
            result += reports.ToString() + ", ";
            result += is_sticky.ToString() + ", ";
            result += is_locked.ToString() + ", ";
            result += is_verified.ToString() + ", ";
            result += "'" + date_modified + "', ";
            result += "'" + date_added + "')"+(last?";":",")+Environment.NewLine;
            return result;
        }
    }
}