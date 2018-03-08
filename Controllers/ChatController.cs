using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using glostars.Glostars_Hub;
using glostars.Models;
using glostars.Models.ChatModel;
using glostars.Models.ChatModel.ChatHelpers;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace glostars.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        public ApplicationDbContext Db = new ApplicationDbContext();
    
        public ActionResult Chat()
        {
            var userList = Db.Users.ToList();
            ViewBag.Users = userList;

            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            var msgList = Db.Conversations.ToList();
            ViewBag.Message = msgList;

            var instantuser = User.Identity.GetUserId();

            var dhur = from s in selectUser
                       where s.UserId1 == instantuser
                       select s;
            ViewBag.Dhur = dhur;

            return View();
        }

        // json request and ajax calling for autosuggestion search
        [HttpPost]
        public JsonResult Search(string prefix)
        {
            
            var logedinuser = User.Identity.GetUserId();
            
            var searchbase2 = Db.ChatRooms.Where(x => x.UserId1 == logedinuser);

            var friendlist = (from f in searchbase2
                            where f.UserName2.StartsWith(prefix)
                            select new {f.UserName2});
            return Json(friendlist, JsonRequestBehavior.AllowGet);
        }

        //json requerst and ajax coaling for new message suggestions search
        [HttpPost]
        public JsonResult NewMessageSearch(string prefix)
        {
            //----------------------------- Logeed In User ID ---------------------------------------->
            
            
            var myid = User.Identity.GetUserId();


            //----------------------------- Tables Defined Variables --------------------------------->

            var usertable = Db.Users.ToList();
            var notificationTable = Db.FollowerNotifications.ToList();

            // ---------------------------- mutual follower data ---------------------->

            //FollowerNotification fs = new FollowerNotification();
            //var fell = Db.FollowerNotifications.Where().ToList();
            //var fellonote = fs.OriginatedById;
            //var following = Db.Users.Where(x => x.Id == fellonote);

            //var friendlistfollowerandmutualfollower = (from f in following
            //                                           where f.Name.StartsWith(prefix)
            //                                           select new { f.Name });


           


            //-------------------------------follower users data -------------------------------------->


            var linqfello = from p in notificationTable
                            where (p.UserId == myid)
                            select p;

            var followerlist = from k in usertable
                               join fe in linqfello
                               on k.Id equals fe.OriginatedById
                               where k.Name.StartsWith(prefix)
                               select new {k.Name};

            ///////////////
            //var followerlist2 = from k in usertable
            //    join fe in linqfello
            //        on k.Id equals fe.OriginatedById
            //    select k;

            

            //-------------------------------following users data -------------------------------------->

            
            var noteid = Db.FollowerNotifications.ToList();

            var linqdata = from i in noteid
                           where (i.OriginatedById == myid)
                           select i;

            var following = Db.Users.ToList();
            var followinglist = from i in following
                                join d in linqdata
                                on i.Id equals d.UserId
                                where i.Name.StartsWith(prefix)
                                select new { i.Name };

            //////////////////
            //var followinglist2 = from i in following
            //                    join d in linqdata
            //                        on i.Id equals d.UserId
            //                    select i;

            //---------------------------------previous messaging part----------------------------------->

            var logedinuser = User.Identity.GetUserId();
            var searchbase2 = Db.ChatRooms.Where(x => x.UserId1 == logedinuser);


            ////////////////
            var friendlist = (from f in searchbase2
                              where f.UserName2.StartsWith(prefix)
                              select new { f.UserName2 });

           
           
           //------------------------generating total table ------------------------>
            ArrayList alldata = new ArrayList();
            foreach (var fl in followerlist)
            {
                alldata.Add(fl);
            }

            foreach (var flo in followinglist)
            {
                alldata.Add(flo);

            }
            foreach (var fr in friendlist)
            {
                alldata.Add(fr);
            }
            return Json(alldata, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChatFriend()
        {
            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            return View();
        }

        public ActionResult UserUrl(int id)
        {
            //this two line is used for featching all of the user of this system ---->
            var userList = Db.Users.ToList();
            ViewBag.Users = userList;

            //this two line is used for fetching user 2 name from chatroom table --->
            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            //this two line is used for fetching all the conversation table data ---->
            var msgList = Db.Conversations.ToList();
            ViewBag.Message = msgList;

            
            var instantuser = User.Identity.GetUserId();
            //var result = from s in stringList
            //where s.Contains("Tutorials") 
            //select s;

            // this linq code is for selectuser for message --->
            var dhur = from s in selectUser
                       where s.UserId1 == instantuser
                       select s;

            ViewBag.Dhur = dhur;

            // this two line is for chatroom number
            var roomNumber = id;
            ViewBag.RoomNumber = roomNumber;

            
            Conversation cd = new Conversation();

            return View(cd);
        }

        [HttpPost]
        public ActionResult ConData(Conversation model, HttpPostedFileBase image1)
        {
            if (ModelState.IsValid)
            {
                if (image1 != null)
                {
                    model.ChatImage = new byte[image1.ContentLength];
                    image1.InputStream.Read(model.ChatImage, 0, image1.ContentLength);
                }
                if (model.Chattext != null)
                {
                    model.Chattext = EncryptionHelper.AesEncryption(model.Chattext);  
                }                                    
                Db.Conversations.Add(model);
                Db.SaveChanges();
                GlostarsHub.Send(model.UserId1,model.Chattext);
                return RedirectToAction("UserUrl", new { id = model.ChatRoomId });
            }
            return RedirectToAction("UserUrl", new { id = model.ChatRoomId });
            //return RedirectToAction("UserUrl", "Chat", new { @id = id });
        }

        //public string Check()
        //{
        //    GlostarsHub.Send("Successfully tested");
        //    return "";
        //}

        [HttpPost]
        public ActionResult SelectUser(ChatRoom model)
        {
            if (ModelState.IsValid)
            {
                Db.ChatRooms.Add(model);
                Db.SaveChanges();
            }
            return Redirect("Chat");
            //return RedirectToAction("UserUrl", new { id = model.ChatRoomId });
        }


        public ActionResult SelectUser2(string userid1, string userid2, string username2)
        {
            var model = new ChatRoom
            {
                UserId1 = userid1,
                UserId2 = userid2,
                UserName2 = username2
            };


            var model4 = Db.ChatRooms.ToList();


            var selectidone = from z in model4
                where (z.UserId1 == userid1 || z.UserId1 == userid2)
                select z.ChatRoomId;

            var selectidtwo = from w in model4
                where (w.UserId2 == userid1 || w.UserId2 == userid2)
                select w.ChatRoomId;

            
            if(ModelState.IsValid)
            {
                int value = 0;
                bool process = true;
                foreach (var idone in selectidone)
                {
                    foreach (var idtwo in selectidtwo)
                    {
                        if (idone == idtwo)
                        {
                            process = false;
                            value = 1;
                        }
                        else
                        {
                            continue;    
                        }
                    }
                    if (value == 1)
                    {
                        break;
                    }
                }

                if (process == true)
                {
                    Db.ChatRooms.Add(model);
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("Chat");

        }

        //Lets edit the message ----->
        [HttpGet]
        public ActionResult Edit(int id, int editmessage)
        {
            //this two line is used for featching all of the user of this system ---->
            var userList = Db.Users.ToList();
            ViewBag.Users = userList;

            //this two line is used for fetching user 2 name from chatroom table --->
            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            //this two line is used for fetching all the conversation table data ---->
            var msgList = Db.Conversations.ToList();
            ViewBag.Message = msgList;


            var instantuser = User.Identity.GetUserId();
            
            // this linq code is for selectuser for message --->
            var dhur = from s in selectUser
                       where s.UserId1 == instantuser
                       select s;

            ViewBag.Dhur = dhur;

            //var msgList2 = Db.Conversations.Where(x => x.ConversationId == editmessage);


            var editedmessage = (from m in msgList
                where m.ConversationId == editmessage
                select m.Chattext).Single();

            //var editmsgdecrypt = EncryptionHelper.AesDecryption(editedmessage.ToString());
            ViewBag.EditMessage = editedmessage;
            //ViewBag.EditMessage = msgList2;


            // this two line is for chatroom number
            var roomNumber = id;
            ViewBag.RoomNumber = roomNumber;

            var convid = editmessage;
            ViewBag.Convid = convid;
    
            Conversation cd = new Conversation();

            return View();
        }

        //edit action with post

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ConversationId,UserId1,Chattext,ChatRoomId")] Conversation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Chattext = EncryptionHelper.AesEncryption(model.Chattext);
                    Db.Entry(model).State = EntityState.Modified;
                    Db.SaveChanges();
                    return RedirectToAction("UserUrl", new {id = model.ChatRoomId});
                }
            }
            catch (DataException)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("UserUrl", new {id = model.ChatRoomId});
        }


        //Lets delete the message------->
        [HttpPost]
        public ActionResult Delete(int id, int deletemessage)
        {
            var mesgid = deletemessage;
            Conversation con = Db.Conversations.Find(mesgid);
            Db.Conversations.Remove(con);
            Db.SaveChanges();
            return RedirectToAction("UserUrl", new {id = con.ChatRoomId});
        }


        //testing the signalr process
        public ActionResult Test()
        {
            var userList = Db.Users.ToList();
            ViewBag.Users = userList;

            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            var msgList = Db.Conversations.ToList();
            ViewBag.Message = msgList;

            var instantuser = User.Identity.GetUserId();

            var dhur = from s in selectUser
                       where s.UserId1 == instantuser
                       select s;
            ViewBag.Dhur = dhur;

            return View();
        }

        //testing the new message search box
        public ActionResult Test2()
        {
            var userList = Db.Users.ToList();
            ViewBag.Users = userList;

            var selectUser = Db.ChatRooms.ToList();
            ViewBag.SelectUsers = selectUser;

            var msgList = Db.Conversations.ToList();
            ViewBag.Message = msgList;

            var instantuser = User.Identity.GetUserId();

            var dhur = from s in selectUser
                       where s.UserId1 == instantuser
                       select s;
            ViewBag.Dhur = dhur;

            return View();
        }

        //this section is for searching existing user in message box which we already messaged ..
        [HttpPost]
        public ActionResult SearchingData(string name)
        {
            var data = Db.ChatRooms.ToList();

            var specificdata = (from spc in data
                where (spc.UserName2 == name)
                select spc.ChatRoomId).Single();

            //var lastmessage = Db.Conversations.Last();
            //ViewBag.LastMessage = lastmessage.Chattext;

            return RedirectToAction("UserUrl", new {id = specificdata});
        }
        
    }

}