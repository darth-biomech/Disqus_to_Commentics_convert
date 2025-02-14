using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Text;
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

        public static void LoadPageFile(string file)
        {
            thisProgram.pagefile = file;
        }

        public static void LoadCommentFile(string file)
        {
            thisProgram.commentFile = file;
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
        public string pagefile;
        public string commentFile;
        
        public int siteID;
        string defaultEmail = "noreply@leavingthecradle.com";
        string siteName = "leavingthecradle";
        string threadIdentifierPart = "000";
        public Dictionary<string, int> bookIDDetectors = new Dictionary<string, int>()
        {
            {"comic",2},
            {"blog",1},
            {"extra_chapter",3}
        };

        
        public void SaveSettings()
        {
            _settingsIni.Write("pageFile", pagefile, "TransferSettings");
            _settingsIni.Write("commentFile", commentFile, "TransferSettings");
            _settingsIni.Write("siteID", siteID.ToString(), "TransferSettings");
            _settingsIni.Write("defaultEmail", defaultEmail, "TransferSettings");
            _settingsIni.Write("siteName", siteName, "TransferSettings");
            _settingsIni.Write("threadIDPart", threadIdentifierPart, "TransferSettings");
            string bookDetectors="";
            foreach (KeyValuePair<string,int> pair in bookIDDetectors)
            {
                bookDetectors += pair.Key + ","+ pair.Value+ ";";
            }
            _settingsIni.Write("bookDetectors", bookDetectors, "TransferSettings");
        }
        public void LoadSettings()
        {
            if (pagefile == String.Empty)
                pagefile = Application.StartupPath;
            TryGetSettingValue("pageFile", ref pagefile);
            TryGetSettingValue("commentFile", ref commentFile);
            TryGetSettingValue("siteID", ref siteID);
            TryGetSettingValue("defaultEmail", ref defaultEmail);
            TryGetSettingValue("siteName", ref siteName);
            string bookDetectors ="";
           
            TryGetSettingValue("threadIDPart", ref bookDetectors);
            if(bookDetectors !="" && bookDetectors.Contains(';') && bookDetectors.Contains(','))
            {
                string[] detectors = bookDetectors.Split(';');
                bookIDDetectors.Clear();
                foreach (string s in detectors)
                {
                    string[] parts = s.Split(',');
                    int id = 0;
                    if (int.TryParse(parts[1], out int bookID))id=bookID;
                    bookIDDetectors.Add(parts[0], id);
                }
            }
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
        public List<String> pageThreads = new List<string>();
        public Dictionary<string, int> threadmapper = new Dictionary<string, int>(); // discus ID vs grawlix ID
        
        public List<String> usersList = new List<string>();
        public Dictionary<string, int> usersMap = new Dictionary<string, int>(); // discus name vs grawlix ID
        
        public List<String> commentsList = new List<string>();
        public Dictionary<string, int> commentsMap = new Dictionary<string, int>(); // discus ID vs grawlix ID

        public List<Comment> formattedComments = new List<Comment>();

        public List<CommentThread> commentThreads = new List<CommentThread>();
        public List<PageDB> pageDb = new List<PageDB>();
        public void Start()
        {
            if (pagefile != String.Empty && commentFile != String.Empty)
            {
                gui.Log("Starting conversion...");

                string pagesData = File.ReadAllText(pagefile);
                string commentsData = File.ReadAllText(commentFile);
                string[] allPages = pagesData.Split('\n');
                string[] allComments = commentsData.Split('\n');

                string outThreads = "INSERT INTO `pages` (`id`, `site_id`, `identifier`, `reference`, `url`, `moderate`, `is_form_enabled`, `date_modified`, `date_added`) VALUES\n";
                
                commentThreads = new List<CommentThread>();
                pageDb = new List<PageDB>();
                
                for (int i = 1; i < allPages.Length; i++)
                {
                    PageDB pagesDB = ParseBook(allPages[i]);
                    pageDb.Add(pagesDB);
                }
                for (int i = 1; i < allComments.Length; i++)
                {
                    CommentThread commentDB = ParseComments(allComments[i]);
                    commentThreads.Add(commentDB);
                }

                for (int i = 0; i < commentThreads.Count; i++)
                {
                    CommentThread com = commentThreads[i];
                    string refer = com.reference;
                    string p_ID = "-1";
                    for (int j = 0; j < pageDb.Count; j++)
                    {
                        string pageTitle = pageDb[j].Title;
                        //gui.Log(pageTitle.ToLower()+" == "+refer.ToLower());
                        if (pageTitle.ToLower() == refer.ToLower())
                        {
                            p_ID = pageDb[j].Id;
                            break;
                        }
                    }
                    com.identifier = p_ID;

                    string fin = "(" + com.id.ToString() + ", "
                                 + com.siteId.ToString() + ", \'"
                                 + com.identifier + "\', \'"
                                 + com.reference + "\', \'"
                                 + com.url + "\', \'"
                                 + com.moderate + "\', 1, \'"
                                 + com.DateModified + "\', \'"
                                 + com.DateCreated + "\'),\n";
                    outThreads += fin;
                }
                
                ExportFileString("Comments_New",outThreads);
                
                gui.Log("Parsing finished! Paste results into sql backups and pray...");
            }
        }

        private PageDB ParseBook(string bookString)
        {
            bookString = bookString.Substring(1, bookString.Length - 2);
            
            string[] parts = bookString.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Replace('\'', ' ').Trim();
            }
            PageDB book = new PageDB();
            book.Id = parts[0].Trim();
            book.Title = parts[1].Trim();
            book.BookId = parts[2].Trim();
            book.MarkerId = parts[3].Trim();
            book.ToneId = parts[4].Trim();
            book.SortOrder = parts[5].Trim();
            book.DateCreated = parts[6].Trim();
            book.DateModified = parts[7].Trim();
            book.DatePublish = parts[8].Trim();
            book.Options = parts[9].Trim();
            book.Description = parts[10].Trim();
            return book;
        }
        private CommentThread ParseComments(string commentString)
        {
            commentString = commentString.Substring(1, commentString.Length - 3);
            
            string[] parts = commentString.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Replace('\'', ' ').Trim();
            }
            CommentThread comments = new CommentThread();
            comments.id = int.Parse(parts[0].Trim());
            comments.siteId = int.Parse(parts[1].Trim());
            comments.identifier = parts[2].Trim();
            comments.reference = parts[3].Trim();
            comments.url = parts[4].Trim();
            comments.moderate = parts[5].Trim();
            comments.isFormEnabled = parts[6].Trim();
            comments.DateModified = parts[7].Trim();
            comments.DateCreated = parts[8].Trim();
            return comments;
        }
        
        private void CreateUsers(XmlNodeList posts)
        {
            Dictionary<string, string> tokenmap = new Dictionary<string, string>();
            foreach (XmlNode thread in posts)
            {
             //   var iD = thread.Attributes["dsq:id"].Value;
                var author = thread["author"];
                if (author != null)
                {
                    string name = author["name"].InnerText;
                    if (!tokenmap.ContainsKey(name) && thread["isDeleted"].InnerText == "false" && thread["isDeleted"].InnerText == "false") 
                    {
                        tokenmap.Add(name, Guid.NewGuid().ToString());
                    }
                }
            }
            usersList.Clear();
            usersMap.Clear();
            int userID = 1;
            foreach (var token in tokenmap)
            {
                
                string item = "("+userID+", 0, 0, '', '', '"+token.Key+"', '"+defaultEmail+"', 'default', '"+token.Value+"', 1, 1, 1, 1, 'html', '127.0.0.1', '2022-11-01 06:41:14', '2022-11-01 06:41:14'),";
                usersList.Add(item);
                usersMap.Add(token.Key,userID);
                gui.Log(item);
                userID += 1;
            }
            usersMap.Add("anon",userID);
        }
        private void MapPosts(XmlNodeList posts)
        {
            formattedComments.Clear();
            commentsMap.Clear();
            commentsList.Clear();
            int cmntID = 0;
            foreach (XmlNode thread in posts)
            {
                //   var iD = thread.Attributes["dsq:id"].Value;
                var author = thread["author"];
                string name = "anon";
                if (author != null)
                    name = author["name"].InnerText;
                string message = thread["message"].InnerText;
                message = message.Replace("<![CDATA[", "");
                message = message.Replace("]]>", "");
                string date = thread["createdAt"].InnerText;
                int userID = -1;
                if (usersMap.ContainsKey(name))
                    userID = usersMap[name];
                int pageID = -1;
                var thrID = thread["thread"];
                var thrParent = thread["parent"];
                string parentID = "none";
                if (thrParent != null)
                {
                    parentID = thrParent.Attributes["dsq:id"].Value;
                }
                bool used = false;
                if (thrID != null)
                {
                    if (threadmapper.ContainsKey(thrID.Attributes["dsq:id"].Value))
                        pageID = threadmapper[thrID.Attributes["dsq:id"].Value];
                }

                if (thread["isDeleted"].InnerText == "false" && thread["isSpam"].InnerText == "false" && pageID > 0)
                {
                    message = message.Replace("'", "&#39;");
                    message = message.Replace("`", "&#96;");
                    cmntID += 1;
                    Comment newComment = new Comment();
                    newComment.authorID = userID;
                    newComment.date = FDate(date);
                    newComment.id = cmntID;
                    newComment.pageId = pageID;
                    newComment.text = message;
                    newComment.commentID = thread.Attributes["dsq:id"].Value;
                    newComment.parentID = parentID;
                    formattedComments.Add(newComment);
                    commentsMap.Add(thread.Attributes["dsq:id"].Value,cmntID);
                }
            }

            foreach (Comment comment in formattedComments)
            {
                int replyto = 0;
                if (comment.parentID != "none")
                    replyto = commentsMap[comment.parentID];
                                            //`id`,          `user_id`,      `page_id`, `website`, `town`, `state_id`, `country_id`, `rating`, `reply_to`, `headline`, `comment`, `reply`, `ip_address`, `is_approved`, `notes`, `is_admin`, `is_sent`, `sent_to`, `likes`, `dislikes`, `reports`, `is_sticky`, `is_locked`, `is_verified`, `date_modified`, `date_added`
                string thecomment = "(" + comment.id + ", " + comment.authorID + ", " + comment.pageId + ", '', '', 0, 0, 0, " + replyto + ", '', '" + comment.text + "', '', '127.0.0.1', 1, 'transfered from Disqus', 0, 1, 0, 0, 0, 0, 0, 0, 1, '" + comment.date + "', '" + comment.date + "'),";
                commentsList.Add(thecomment);
                gui.Log(thecomment);
            }
        }


        private void MapThreads(XmlNodeList threads)
        {
            threadmapper.Clear();
            pageThreads.Clear();
                int threadcount = 1;
                foreach (XmlNode thread in threads)
                {
                    var iD = thread.Attributes["dsq:id"].Value;
                    var link = thread["link"];
                    if (iD != null && link != null)
                    {
                        string[] linkelem = link.InnerText.Split('/');
                        if (linkelem.Length != 0)
                        {
                            if (linkelem[2].Contains(siteName))
                            {
                                if (linkelem.Length >= 5)
                                {
                                    string pageID = "0";

                                    foreach (KeyValuePair<string,int> pair in bookIDDetectors)
                                    {
                                        if (linkelem[3].ToLower() == pair.Key.ToLower())pageID = pair.Value.ToString();
                                        break;
                                    }
                                    int identifier = 0;
                                    try
                                    {
                                        identifier = Int32.Parse(pageID + threadIdentifierPart + linkelem[4]);
                                    }
                                    catch
                                    {
                                        identifier = 90000 + threadcount;
                                    }

                                    // gui.Log(iD + "  -  " + linkelem[3] + "/" + linkelem[4]);
                                                    //`id`,             `site_id`, `identifier`,    `reference`,          `url`,      `moderate`, `is_form_enabled`, `date_modified`,          `date_added`
                                    string item =   "("+threadcount+",  "+
                                                    siteID+", '"+
                                                    identifier+"', '"+
                                                    linkelem[4]+"' , '"+
                                                    link.InnerText+
                                                    "' , 'default', 1, '"+
                                                    FDate(thread["createdAt"].InnerText)+"', '"+
                                                    FDate(thread["createdAt"].InnerText)+"'),";
                                    pageThreads.Add(item);
                                    gui.Log(item);
                                    threadmapper.Add(iD,threadcount);
                                    threadcount += 1;
                                }
                            }
                        }
                    }
                }
        }


        private string FDate(string inp)
        {
            string date = inp;
            //2021-08-24T17:34:28Z
            //2022-11-01 06:41:14
            date = date.Replace("Z", "");
            date = date.Replace("T", " ");
            return date;
        }

        public void ExportFileString(string filename, string content)
        {
            string filepath = Path.GetDirectoryName(commentFile)+"/"+filename+".txt";
            
            File.Delete(filepath);
            File.WriteAllText(filepath, content);
            gui.Log("Saving '"+filename+".sql' into output directory");
        }
        public void ExportFile(string filename, List<String> content)
        {
            string filepath = commentFile+"/"+filename+".sql";
            string outputString = " ";
            for (int i = 0; i < content.Count; i++)
            {
                string cont = content[i];
                if (i == content.Count-1)
                {
                    cont = cont.Replace("'),", "');");
                }

                outputString += cont;
                outputString += Environment.NewLine;
            }
            File.Delete(filepath);
            File.WriteAllText(filepath, outputString);
            gui.Log("Saving '"+filename+".sql' into output directory");
        }
    }
}