using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppECartDemo.Models;
using WebAppECartDemo.ViewModel;

namespace WebAppECartDemo.Controllers
{
    public class ItemController : Controller
    {
        private ECartDbEntities objEcartDbEntities;
        public ItemController()
        {
            objEcartDbEntities= new ECartDbEntities();
        }
        // GET: Item
        public ActionResult Index()
        {
            /*ItemViewModel itemViewModel = new ItemViewModel();
            itemViewModel.CategorySelectListItem = (from objcat in objEcartDbEntities.Categories
                                                    select new SelectListItem()
                                                    {
                                                        Text = objcat.CategoryName,
                                                        Value = objcat.CategoryId.ToString(),
                                                        Selected = true
                                                    });
            return View(itemViewModel);*/
            ItemViewModel itemViewModel = new ItemViewModel();

            // Populate the SelectListItems from the database
            itemViewModel.CategorySelectListItem = objEcartDbEntities.Categories
                .Select(objcat => new SelectListItem
                {
                    Text = objcat.CategoryName,
                    Value = objcat.CategoryId.ToString(),
                    Selected = true
                });

            return View(itemViewModel);
        }
        [HttpPost]
        public JsonResult Index(ItemViewModel objItemViewModel)
        {
            string imagePath = null;

            /* string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
             objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));


             Item objItem = new Item();
             objItem.ImagePath = "~/Images/" + NewImage;
             objItem.CategoryId = objItemViewModel.CategoryId;
             objItem.Description = objItemViewModel.Description;
             objItem.ItemCode = objItemViewModel.ItemCode;
             objItem.ItemId = Guid.NewGuid();
             objItem.ItemName = objItemViewModel.ItemName;
             objItem.ItemPrice = objItemViewModel.ItemPrice;

             objEcartDbEntities.Items.Add(objItem);
             objEcartDbEntities.SaveChanges();

             return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);*/
            if (objItemViewModel.ImagePath != null && objItemViewModel.ImagePath.ContentLength > 0)
            {
                
                // Generate a unique filename for the uploaded image
                string newImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
                // Save the image to the server
                objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + newImage));

                // Update the objItemViewModel with the new image path
               imagePath = "~/Images/" + newImage;
            }

            Item objItem = new Item
            {
               ImagePath = imagePath,
                CategoryId = objItemViewModel.CategoryId,
                Description = objItemViewModel.Description,
                ItemCode = objItemViewModel.ItemCode,
                ItemId = Guid.NewGuid(),
                ItemName = objItemViewModel.ItemName,
                ItemPrice = objItemViewModel.ItemPrice
            };

            objEcartDbEntities.Items.Add(objItem);
            objEcartDbEntities.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Successfully." }, JsonRequestBehavior.AllowGet);
        }
    }
    
}
