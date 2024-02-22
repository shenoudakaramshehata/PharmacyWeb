using Pharmacy.Data;
using Pharmacy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.Entities;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Pharmacy.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]


    public class MobileController : Controller
    {

        private readonly PharmacyContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;


        public MobileController(PharmacyContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;


        }

        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string UserName, [FromQuery] string Password)
        {

            var user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null && roles.FirstOrDefault() == "Customer")
                    {
                        var customer = await _context.Customers.FindAsync(user.EntityId);

                        return Ok(new { Status = "Success", Message = "User Login successfully!", user, customer });
                    }
                }
            }
            var invalidResponse = new { status = false };
            return Ok(invalidResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationModel Model)   
        {
            var userExists = await _userManager.FindByNameAsync(Model.UserName);
            if (userExists != null)

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            var customer = new Customer();
            if (Model.CustomerImage != null)
            {
                var bytes = Convert.FromBase64String(Model.CustomerImage);
                string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Customer");
                string uniqePictureName = Guid.NewGuid().ToString("N") + ".jpeg";
                string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                customer.CustomerImage = uniqePictureName;
            }
            customer.CustomerAddress = Model.CustomerAddress;
            customer.CustomerNameAr = Model.CustomerNameAr;
            customer.CustomerNameEn = Model.CustomerNameEn;
            customer.CustomerPhone = Model.CustomerPhone;
            customer.CustomerRemarks = Model.CustomerRemarks;
            customer.CustomerRemarks = Model.CustomerRemarks;
            customer.CustomerEmail = Model.CustomerEmail;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            if (!(customer.CustomerId > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }


            var user = new ApplicationUser
            {
                UserName = Model.UserName,
                Email = Model.CustomerEmail,
                PhoneNumber = Model.CustomerPhone,
                EntityId = customer.CustomerId,
                EntityName = "Customer"

            };

            var result = await _userManager.CreateAsync(user, Model.Password);

            if (!result.Succeeded)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }
            await _userManager.AddToRoleAsync(user, "Customer");
            return Ok(new { Status = "Success", Message = "User created successfully!", user, customer });

        }
        [HttpGet]
        public async Task<IActionResult> GetCartInfoByCustomerId([FromQuery] int CustomerId)
        {



            var cartLst = _context.Carts.Where(c => c.CustomerId == CustomerId).Select(j => new
            {
                j.CartId,
                j.CustomerId,
                j.ItemId,
                j.QTY,
                j.UnitPrice,
                j.Total,
                j.CartDate,
                j.Remarks,
                item = j.Item,
                //itemImage = j.Item.ItemImage

            });

           var totalItem = cartLst.Count();
            var totalPrice=  _context.Carts.Where(c => c.CustomerId == CustomerId).Select(c=>c.Total).Sum();

            
            return Ok(new { totalItem, totalPrice, cartLst });



        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart model)
        {
            model.CartDate = DateTime.Now;
            _context.Carts.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { Message = " Item Added To Cart" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFromCart([FromQuery] int cartId)
        {
            var model = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            if (model==null)
            {
                return Ok(new { Message = "  Item Not Found" });
            }
            _context.Carts.Remove(model);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "  Item Deleted From Cart" });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] Cart model)
        {
            var cartModel = await _context.Carts.AnyAsync(c => c.CartId == model.CartId);
            if (cartModel == false)
            {
                return Ok(new { Message = "  Item Not Found" });
            }
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Item Updated" });

        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo([FromQuery] int CustomerId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == CustomerId);
            if (customer ==null)
            {
                return Ok(new { Message = "customer Not Found" });
            }
            return Ok(new { customer });
        }
        //Home Methods
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string SearchText, [FromQuery] int CustomerId)

        {
            if (CustomerId == 0)
            {

                var items = _context.Items.Where(c => c.ItemTlAr.Contains(SearchText) || c.ItemTlEn.Contains(SearchText) || c.Category.CategoryTlAr.Contains(SearchText) || c.Category.CategoryTlEn.Contains(SearchText) || c.Description.Contains(SearchText) || c.Brand.BrandTlAr.Contains(SearchText) || c.Brand.BrandTlEn.Contains(SearchText)).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    i.ItemTlEn,i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.CategoryId,
                    i.Description,
                    i.Remarks,
                    i.IsActive,
                    IsFav = false
                });
                return Ok(new { items });
            }
            else
            {
                var items = _context.Items.Where(c => c.ItemTlAr.Contains(SearchText) || c.ItemTlEn.Contains(SearchText) || c.Category.CategoryTlAr.Contains(SearchText) || c.Category.CategoryTlEn.Contains(SearchText) || c.Description.Contains(SearchText) || c.Brand.BrandTlAr.Contains(SearchText) || c.Brand.BrandTlEn.Contains(SearchText)).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    i.ItemTlEn,i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.CategoryId,
                    i.Description,
                    i.Remarks,
                    i.IsActive,
                    IsFav = _context.CustomerFav.Any(c => c.CustomerId == CustomerId && c.ItemId == i.ItemId)
                });
                return Ok(new { items });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetHomeSlider()
        {

            var sliderList = await _context.HomeSliders.ToListAsync();

            return Ok(new { sliderList });
        }
        [HttpGet]
        public async Task<IActionResult> GetSectionsList()
        {
            var sectionList = await _context.Sections.ToListAsync();

            return Ok(new { sectionList });
        }
        [HttpGet]
        public async Task<IActionResult> GetCollectionsList([FromQuery] int CustomerId)
        {
            if (CustomerId == 0)
            {
                var collectionLst = _context.Collections.Select(i => new
                {
                    i.CollectionId,
                    i.CollectionTlAr,
                    i.CollectionTlEn,
                    i.CollectionSort,
                    i.IsActive,
                    Items = i.CollectionItem.Select(b => b.Item).Select(m => new
                    {
                        m.ItemId,
                        m.ItemTlAr,
                       m.ItemPic,
                        m.ItemTlEn,
                        m.Price,
                        m.BrandId,
                        m.Description,
                        m.Remarks,
                        m.IsActive,
                        m.CategoryId,
                        IsFav = false,
                        //itemImage=m.ItemImage
                    }),
                });

                return Ok(new { collectionLst });
            }
            else
            {
                var collectionLst = _context.Collections.Select(i => new
                {
                    i.CollectionId,
                    i.CollectionTlAr,
                    i.CollectionTlEn,
                    i.CollectionSort,
                    i.IsActive,
                    Items = i.CollectionItem.Select(b => b.Item).Select(m => new
                    {
                        m.ItemId,
                        m.ItemTlAr,
                        m.ItemPic,
                        m.ItemTlEn,
                        m.Price,
                        m.BrandId,
                        m.Description,
                        m.Remarks,
                        m.IsActive,
                        m.CategoryId,
                        //itemImage = m.ItemImage,
                        IsFav = _context.CustomerFav.Any(c => c.CustomerId == CustomerId && c.ItemId == m.ItemId)
                    }),
                });

                return Ok(new { collectionLst });






            }

        }
        
        [HttpGet]
        public async Task<IActionResult> GetItemsByCollectionID([FromQuery] int CollectionId, [FromQuery] int CustomerId)
        {

            if (CustomerId == 0)
            {
                var collectionLst = _context.Collections.Where(c => c.CollectionId == CollectionId).Select(i => new
                {
                    i.CollectionId,
                    i.CollectionTlAr,
                    i.CollectionTlEn,
                    i.CollectionSort,
                    i.IsActive,
                    Items = i.CollectionItem.Select(b => b.Item).Select(m => new
                    {
                        m.ItemId,
                        m.ItemTlAr,
                        m.ItemPic,
                        m.ItemTlEn,
                        m.Price,
                        m.BrandId,
                        m.Description,
                        m.Remarks,
                        m.IsActive,
                        m.CategoryId,
                        IsFav = false
                    }),
                });

                return Ok(new { collectionLst });
            }
            else
            {
                var collectionLst = _context.Collections.Where(c => c.CollectionId == CollectionId).Select(i => new
                {
                    i.CollectionId,
                    i.CollectionTlAr,
                    i.CollectionTlEn,
                    i.CollectionSort,
                    i.IsActive,
                    Items = i.CollectionItem.Select(b => b.Item).Select(m => new
                    {
                        m.ItemId,
                        m.ItemTlAr,
                        m.ItemPic,
                        m.ItemTlEn,
                        m.Price,
                        m.BrandId,
                        m.Description,
                        m.Remarks,
                        m.IsActive,
                        m.CategoryId,
                        IsFav = _context.CustomerFav.Any(c => c.CustomerId == CustomerId && c.ItemId == m.ItemId)
                    }),
                });

                return Ok(new { collectionLst });

            }









        }
        //Category Page
        //[HttpGet]
        //public async Task<IActionResult> GetCategoriesBySectionID([FromQuery] int SectionId)
        //{
        //    var categoryList = await _context.Categories.Where(c => c.SectionId == SectionId).ToListAsync();

        //    return Ok(new { categoryList });
        //}
         [HttpGet]
        public async Task<IActionResult> GetCategoriesList()
        {
            var categoryList = await _context.Categories.ToListAsync();
            return Ok(new { categoryList });
        }
        //Items Page
        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var BrandList = await _context.Brands.ToListAsync();
            return Ok(new { BrandList });
        }
        [HttpGet]
        public async Task<IActionResult> GetItemsByCategory([FromQuery] int CategoryId, [FromQuery] int CustomerId)
        {
            if (CustomerId == 0)
            {
                var items = _context.Items.Where(c => c.CategoryId == CategoryId).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    i.ItemTlEn,i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.CategoryId,
                    i.Description,
                    i.Remarks,
                    i.IsActive,
                    //itemImage = i.ItemImage,

                    IsFav = false
                });
                return Ok(new { items });

            }
            else
            {
                var items = _context.Items.Where(c => c.CategoryId == CategoryId).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    i.ItemTlEn,i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.Description,
                    i.CategoryId,
                    i.Remarks,
                    i.IsActive,
                    //itemImage = i.ItemImage,
                    IsFav = _context.CustomerFav.Any(c => c.CustomerId == CustomerId && c.ItemId == i.ItemId)
                });
                return Ok(new { items });

            }
        }
        //Item
        [HttpGet]
        public async Task<IActionResult> GetItemById([FromQuery] int ItemId, [FromQuery] int CustomerId)
        {
            if (CustomerId == 0)
            {
                var items = _context.Items.Where(c => c.ItemId == ItemId).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    i.ItemTlEn,i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.CategoryId,
                    i.Description,
                    i.Remarks,
                    i.IsActive,
                    IsFav = false
                });
                return Ok(new { items });

            }
            else
            {
                var items = _context.Items.Where(c => c.ItemId == ItemId).Select(i => new
                {
                    i.ItemId,
                    i.ItemTlAr,
                    
                    i.ItemTlEn,
                    i.ItemPic,
                    i.Price,
                    i.BrandId,
                    i.Brand.BrandTlAr,
                    i.Brand.BrandTlEn,
                    i.CategoryId,
                    i.Description,
                    i.Remarks,
                    i.IsActive,
                    IsFav = _context.CustomerFav.Any(c => c.CustomerId == CustomerId && c.ItemId == i.ItemId)
                });
                return Ok(new { items });

            }
        }
        [HttpGet]
        public async Task<IActionResult> GetItemImagesById([FromQuery] int ItemId)
        {

            var ItemImageList = await _context.ItemImages.Where(c => c.ItemId == ItemId).ToListAsync();

            return Ok(new { ItemImageList });

        }
        //ShoppingCart
        [HttpGet]
        public async Task<IActionResult> GetShopingCartByCustomerId([FromQuery] int CustomerId)
        {
            var ShopingCarList = await _context.Carts.Where(c => c.CustomerId == CustomerId).ToListAsync();

            return Ok(new { ShopingCarList });
        }
       // Configuration
        [HttpGet]
        public async Task<IActionResult> GetSystemConfiguration()
        {
            var SystemConfiguration = await _context.Configurations.FirstOrDefaultAsync();
            return Ok(new { SystemConfiguration });
        }
        [HttpGet]
        public async Task<IActionResult> GetFAQList()
        {
            var SystemConfiguration = await _context.FAQs.ToListAsync();
            return Ok(new { SystemConfiguration });
        }
        [HttpGet]
        public async Task<IActionResult> GetPageContent([FromQuery] int PageContentId)
        {
            var SystemConfiguration = await _context.PageContent.FirstOrDefaultAsync(c=>c.PageContentId== PageContentId);
            return Ok(new { SystemConfiguration });
        }
        //Favorite
        [HttpGet]
        public async Task<IActionResult> GetFavListByCustomerId([FromQuery] int CustomerId)
        {
            var FavList = await _context.CustomerFav.Where(c => c.CustomerId == CustomerId).Select(c=>c.ItemId).ToListAsync();
            var FavItems = _context.Items.Where(c => FavList.Contains(c.ItemId)).Select(i => new
            {
                i.ItemId,
                i.ItemTlAr,
                i.ItemTlEn,
                i.ItemPic,
                i.Price,
                i.BrandId,
                i.Brand.BrandTlAr,
                i.Brand.BrandTlEn,
                i.CategoryId,
                i.Description,
                i.Remarks,
                i.IsActive,
                //itemImage = i.ItemImage,
                IsFav = true
            });
            
            return Ok(new { FavItems });

        }
        [HttpPost]
        public async Task<IActionResult> AddFav([FromQuery] int CustomerId, [FromQuery] int ItemId)
        {
            var model = await _context.CustomerFav.Where(c => c.CustomerId == CustomerId && c.ItemId == ItemId).FirstOrDefaultAsync();
            if (model != null)
            {
                return Ok(new { Message = "Item Already exist In Fav List " });

            }
            var customerFav = new CustomerFav();
            customerFav.CustomerId = CustomerId;
            customerFav.ItemId = ItemId;
            _context.CustomerFav.Add(customerFav);
            _context.SaveChanges();

            return Ok(new { Message = " Fav Item Added" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFav([FromQuery] int CustomerId, [FromQuery] int ItemId)
        {
            var model = await _context.CustomerFav.FirstOrDefaultAsync(c => c.ItemId == ItemId && c.CustomerId == CustomerId);
            if (model == null)
            {
                return Ok(new { Message = "  Item Not Found" });
            }
            _context.CustomerFav.Remove(model);
            await _context.SaveChangesAsync();
            return Ok(new { Message = " Fav Item Deleted" });

        }
        //Contact Us
        [HttpPost]
        public async Task<IActionResult> ContactUs([FromBody] ContactUs Model)
        {
            await _context.ContactUs.AddAsync(Model);
             _context.SaveChanges();

            return Ok(new { Message = "Message Sent Successfully" });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody]  ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return Ok(new { Message = "model not Valid" });
            if (resetPasswordModel.ConfirmPassword!=resetPasswordModel.NewPassword)
            {
                return Ok(new { Message = "Confirm Password and New Password not matched" });
            }

            var curUser = _userManager.Users.Where(c => c.EntityId == resetPasswordModel.CustomerId).FirstOrDefault();
            if (curUser == null)
            {
                return StatusCode(409, "Object not found");
            }
            var Result = await _userManager.ChangePasswordAsync(curUser,resetPasswordModel.CurrentPassword,resetPasswordModel.NewPassword);
            if (!Result.Succeeded)
            {
                foreach (var error in Result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return Ok(new { Message = ModelState });

            }

            return Ok(new { Message = "Password Changed" });

        }
        [HttpPost]
        public async Task<IActionResult> EditCustomer([FromBody] Customer model)
        {
            try
            {
                Customer customer = _context.Customers.FirstOrDefault(e => e.CustomerId == model.CustomerId);
                if (customer == null)
                    return Ok(new { Message = "Customer Not Found" });

                var wwwroot = _hostEnvironment.WebRootPath;
                var ImagePath = Path.Combine(wwwroot, "Images/Customer/" + customer.CustomerImage);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                if (model.CustomerImage != null)
                {
                    var bytes = Convert.FromBase64String(model.CustomerImage);
                    string uploadFolder = Path.Combine(_hostEnvironment.WebRootPath, "Images/Customer");
                    string uniqePictureName = Guid.NewGuid().ToString("N") + ".jpeg";
                    string uploadedImagePath = Path.Combine(uploadFolder, uniqePictureName);
                    using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    customer.CustomerImage = uniqePictureName;
                }

                customer.CustomerAddress = model.CustomerAddress;
                customer.CustomerNameAr = model.CustomerNameAr;
                customer.CustomerNameEn = model.CustomerNameEn;
                customer.CustomerPhone = model.CustomerPhone;
                customer.CustomerAddress = model.CustomerAddress;
                _context.Attach(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { customer });

            }
            catch (Exception ex)
            {

                throw ex;
            }
           

        }
        [HttpPost]
        public ActionResult<object> Addorder([FromQuery] int CustomerId , [FromQuery] int PaymentMethodId,int AreaId)
        {
            try
            {
                var cartLst = _context.Carts.Where(s => s.CustomerId == CustomerId).ToList();

                if (cartLst.Count() > 0)
                {
                    var totalItem = cartLst.Count();
                    var totalPrice = cartLst.Select(c => c.Total).Sum();

                    var area = _context.Area.Find(AreaId);
                    if (area == null)
                    {
                        return Ok(new { status = false, Message = "creation failed! Please Select area  and try again." });
                    }
                    Order order = new Order();
                    order.CustomerId = CustomerId;
                    order.PaymentMethodId = PaymentMethodId;
                    order.Addrerss = _context.Customers.Where(c => c.CustomerId == CustomerId).FirstOrDefault().CustomerAddress;
                    order.OrderDate = DateTime.Now;
                    order.Total = totalPrice + area.DeliveryCost;
                    order.OrderSerial = Guid.NewGuid().ToString("N") + "/" + DateTime.Now.Year;
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var item in cartLst)
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.ItemId = item.ItemId.Value;
                        orderItem.ItemPrice = item.UnitPrice;
                        orderItem.Qty = item.QTY;
                        orderItem.OrderId = order.OrderId;
                        orderItem.Total = item.Total;
                        _context.OrderItems.Add(orderItem);
                    }
                    _context.Carts.RemoveRange(cartLst);
                    _context.SaveChanges();


                    return Ok(new { Message = "Your Order has Been Added" });


                }

                return Ok(new { Message = "Shopping Cart Is Empty" });

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        public ActionResult<object> GetordersByCustomerId([FromQuery] int CustomerId)
        {
            var OrerLst = _context.Orders.Where(c =>c.CustomerId== CustomerId).Select(i => new
            {
                i.Addrerss,
                i.Closed,
                i.CustomerId,
                i.OrderDate,
                i.OrderId,
                i.OrderSerial,
                i.PaymentMethodId,
                i.Remarks,
                i.Total,
                OrderItem = i.OrderItem,
                ItemDetails = i.OrderItem.Select(j=>new
                {
                    j.Item.ItemPic,
                    j.Item.ItemTlAr,
                    j.Item.ItemTlEn,
                    j.Item.Description
                }
                    )

        });

            return Ok(new { OrerLst });

        }
        [HttpGet]
        public ActionResult<object> GetPaymentMethodsList()
        {
            var PaymentLst = _context.PaymentMethods.ToList();
            return Ok(new { PaymentLst });

        }
        [HttpGet]
        public IActionResult GetAreasList()
        {
            try
            {
                var list = _context.Area.Where(c => c.AreaIsActive == true && c.City.CityIsActive == true).OrderBy(c => c.AreaOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult GetAreasListByCityId(int cityId)
        {
            try
            {
                var list = _context.Area.Where(c => c.AreaIsActive == true &&c.CityId==cityId && c.City.CityIsActive == true).OrderBy(c => c.AreaOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {
                return Ok(new { status = false, message = "Something went wrong" });
            }

        }
        [HttpGet]
        public IActionResult GetCitiesList()
        {
            try
            {
                var list = _context.City.Include(c => c.Area.Where(c => c.AreaIsActive == true)).Where(c => c.CityIsActive == true ).OrderBy(c => c.CityOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }


    }
}


