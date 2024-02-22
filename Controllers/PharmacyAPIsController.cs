using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using Newtonsoft.Json;
using Pharmacy.Data;
using Pharmacy.Entities;
using Pharmacy.Models;
using Pharmacy.Services;
using Pharmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Pharmacy.Controllers
{
    [Route("api/[Controller]/[action]")]

    public class PharmacyAPIsController : Controller
    {
        private readonly PharmacyContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;
        public HttpClient httpClient { get; set; }
        public InvoiceVm invoiceVm { get; set; }
        public Customer Customer { get; set; }
        public Delivery delivery { get; set; }

        private readonly IRazorPartialToStringRenderer _renderer;
        
        public PharmacyAPIsController(IRazorPartialToStringRenderer renderer, PharmacyContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            ApplicationDbContext db, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment, IEmailSender emailSender)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _emailSender = emailSender;
            httpClient = new HttpClient();
           
            _renderer = renderer;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerAccount(int customerId)
        {
            var user = _db.Users.Where(e => e.EntityId == customerId).FirstOrDefault();
            try
            {
                if (user == null)
                {
                    return Ok(new { Status = false, Message = "Customer Not Found" });
                }

                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(new { Status = false, Message = e.Message });

            }
            return Ok(new { Status = true, Message = "Customer Account Deleted Successfully" });
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationModel Model)
        {
            var userExists = await _userManager.FindByNameAsync(Model.CustomerEmail);
            if (userExists != null)

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            var customer = new Customer();
            if (Model.CustomerImage != string.Empty)
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
            customer.CustomerEmail = Model.CustomerEmail;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            if (!(customer.CustomerId > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            }


            var user = new ApplicationUser
            {
                UserName = Model.CustomerEmail,
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
        public async Task<ActionResult<ApplicationUser>> Login([FromQuery] string Email, [FromQuery] string Password)
        {

            var user = await _userManager.FindByNameAsync(Email);

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
        public async Task<ActionResult<ApplicationUser>> forgetPassword([FromQuery] string UserName)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(UserName);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesList()
        {
            var categoryList = await _context.Categories.Where(e => e.IsActive == true).ToListAsync();
            return Ok(new { categoryList });
        }
        [HttpGet]
        public async Task<IActionResult> getItemsImageList()
        {
            var itemsImageList = await _context.ItemImages.Include(e => e.Item).Where(e => e.Item.IsActive == true).ToListAsync();
            return Ok(new { itemsImageList });
        }
        [HttpGet]
        public async Task<IActionResult> getItemsList()
        {
            var ItemsList = await _context.Items.Where(e => e.IsActive == true && e.Category.IsActive == true).Select(e => new
            {
                e.ItemTlAr,
                e.ItemTlEn,
                e.Price,
                e.Remarks,
                e.Description,
                e.Stock,
                e.ItemId,
                e.ItemPic,
                e.CategoryId,
                e.BrandId,
             Images= _context.ItemImages.Where(i=>i.ItemId==e.ItemId).ToList()
			}).ToListAsync();
            return Ok(new { ItemsList });
        }
        [HttpGet]
        public async Task<IActionResult> getItembyId(int id)
        {
            var Item = await _context.Items.Where(i => i.ItemId == id).FirstOrDefaultAsync();
            return Ok(new { Item });
        }

        [HttpGet]
        public IActionResult getitemsbycategoryId(int? CategoryId)
        {
            if (CategoryId != null)
            {
                var items = _context.Items.Where(i => i.CategoryId == CategoryId && i.IsActive == true && i.Category.IsActive == true);
                return Ok(new { items });
            }
            else
            {
                return Ok(new { Message = "Category Id can not be null" });
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> categorybysectionid(int id)
        //{
        //    var categoryList = await _context.Categories.Where(x => x.SectionId == id).ToListAsync();
        //    return Ok(new { categoryList });
        //}

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string SearchText, [FromQuery] int CustomerId = 0)

        {
            if (CustomerId == 0)
            {

                var items = await _context.Items.Where(c => c.IsActive == true && c.Category.IsActive == true && (c.ItemTlAr.Contains(SearchText) || c.ItemTlEn.Contains(SearchText) || c.Category.CategoryTlAr.Contains(SearchText) || c.Category.CategoryTlEn.Contains(SearchText) || c.Description.Contains(SearchText) || c.Brand.BrandTlAr.Contains(SearchText) || c.Brand.BrandTlEn.Contains(SearchText))).Select(i => new
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
                    IsFav = false
                }).ToListAsync();
                return Ok(new { items });
            }
            else
            {
                var items = await _context.Items.Where(c => c.IsActive == true && c.Category.IsActive == true && (c.ItemTlAr.Contains(SearchText) || c.ItemTlEn.Contains(SearchText) || c.Category.CategoryTlAr.Contains(SearchText) || c.Category.CategoryTlEn.Contains(SearchText) || c.Description.Contains(SearchText) || c.Brand.BrandTlAr.Contains(SearchText) || c.Brand.BrandTlEn.Contains(SearchText))).Select(i => new
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
                }).ToListAsync();
                return Ok(new { items });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeSlider()
        {

            //var sliderList = await _context.HomeSliders.ToListAsync();
            var sliderList = await _context.HomeSliders.Select(h => new { h.HomeSliderPic }).ToListAsync();

            return Ok(new { sliderList });
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var BrandList = await _context.Brands.ToListAsync();
            return Ok(new { BrandList });
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerInfo([FromQuery] int CustomerId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == CustomerId);
            if (customer == null)
            {
                return Ok(new { Message = "customer Not Found" });
            }
            return Ok(new { customer });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromQuery] int CustomerId, int ItemId, int QTY)
        {
            Item item = _context.Items.Find(ItemId);
            Cart model = new Cart();
            model.CartDate = DateTime.Now;
            model.CustomerId = CustomerId;
            model.ItemId = ItemId;
            model.UnitPrice = item.Price;
            model.QTY = QTY;
            model.Total = item.Price * QTY;
            _context.Carts.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { Message = " Item Added To Cart" });
        }

        [HttpPost]
        public async Task<IActionResult> deleteCartItem(int id)
        {
            Cart cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return Ok(new { Message = " Item Deleted" });
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentList()
        {
            List<PaymentMethod> payments = await _context.PaymentMethods.ToListAsync();

            return Ok(new { payments });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderList(int userId)
        {

            var user = await _context.Customers.FindAsync(userId);
            if (user != null)
            {
                var OrerLst = _context.Orders.Where(c => c.CustomerId == userId).Select(i => new
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
                    ItemDetails = i.OrderItem.Select(j => new
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
            else
                return Ok(new { Message = " user Not found" });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationslist(int userId)
        {
            List<Notifications> lstNotifications = await _context.Notifications.Where(n => n.CustomerId == userId).ToListAsync();

            return Ok(new { lstNotifications });
        }

        [HttpGet]
        public async Task<IActionResult> allOrderList()
        {
            List<Order> lstOrders = new List<Order>();
            lstOrders = await _context.Orders.ToListAsync();
            return Ok(new { lstOrders });
        }
        [HttpGet]
        public IActionResult Top5OrderedItems()
        {
            var topItemsIds = _context.OrderItems.Include(e => e.Item).Where(i => i.Item.IsActive == true && i.Item.Category.IsActive == true).GroupBy(x => x.ItemId).OrderByDescending(g => g.Count()).Take(5).Select(x => x.Key).ToList();
            var topItems = _context.Items.Where(x => topItemsIds.Contains(x.ItemId));
            return Ok(new { topItems });
        }

        [HttpPost]
        public async Task<IActionResult> makeOrder([FromBody] Order model,int areaId) {

             if (model != null)
                {
                foreach (var orderItem in model.OrderItem)
                {
                    var item = _context.Items.Find(orderItem.ItemId);
                    if (item.Stock <= 0 || item.Stock == null)
                    {
                        return Ok(new { Status = false, Message = item.ItemTlEn + " is Out of Stock" });

                    }
                    if (item.Stock < orderItem.Qty)
                    {
                        return Ok(new { Status = false, Message = item.ItemTlEn + " Quantity less than Stock Quantity" });

                    }

                    
                }

                model.OrderDate = DateTime.Now;
                if (_context.Orders.ToList().Count() == 0)
                {
                    model.OrderSerial = Convert.ToString(1);
                }
                else
                {
                    var maxserial = _context.Orders.ToList().Max(e => Convert.ToInt64(e.OrderSerial));
                    model.OrderSerial = Convert.ToString(maxserial + 1);
                }
                var area = _context.Area.Find(areaId);
                var deliverycost = area.DeliveryCost;
                model.Closed = false;
                model.ispaid = false;
                model.DeliveryId = 1;
                model.DeliveryCost = deliverycost;
                try
                {
                    _context.Orders.Add(model);
                    _context.SaveChanges();

                }
                catch (Exception e)
                {
                    return Ok(new { Status = false, Message = e.Message });
                }
                var Customer = _context.Customers.Find(model.CustomerId);
                
                if (area == null)
                {
                    return Ok(new { status = false, Message = "creation failed! Please Select area  and try again." });
                }
               

                if (model.PaymentMethodId == 2)
                {
                    var requesturl = "https://api.upayments.com/test-payment";
                    var fields = new
                    {
                        merchant_id = "1201",
                        username = "test",
                        password = "test",
                        order_id = model.OrderId,
                        total_price = model.Total + area.DeliveryCost,
                        test_mode = 0,
                        CstFName = Customer.CustomerNameEn,
                        CstEmail = Customer.CustomerEmail,
                        CstMobile = Customer.CustomerPhone,
                        api_key = "jtest123",
                        //success_url =/* "https://localhost:44354/success"*/,
                        success_url = "http://codewarenet-001-site17.dtempurl.com/success",
                        //error_url = /*"https://localhost:44354/failed"*/
                        error_url = "http://codewarenet-001-site17.dtempurl.com/failed"

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
                    var task = httpClient.PostAsync(requesturl, content);
                    var result = await task.Result.Content.ReadAsStringAsync();
                    var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
                    if (paymenturl.status == "success")

                    {
                        return Ok(new { Status = "true", paymenturl = paymenturl.paymentURL, order = model.OrderItem });
                    }
                    else
                    {
                        _context.Orders.Remove(model);
                        _context.SaveChanges();
                        return Ok(new { Status = "false", reason = paymenturl.error_msg });
                    }
                }
                //if (model.PaymentMethodId == 1)
                //{
                //    foreach (var orderItem in model.OrderItem)
                //    {
                //        var item = _context.Items.Find(orderItem.ItemId);
                //        item.Stock -= orderItem.Qty;
                //        var UpdatedItem = _context.Items.Attach(item);
                //        UpdatedItem.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //        _context.SaveChanges();

                //    }
                //    if (model.CustomerId != null)
                //    {
                //        var carts = _context.Carts.Where(e => e.CustomerId == model.CustomerId);
                //        _context.Carts.RemoveRange(carts);
                //        _context.SaveChanges();
                //    }
                //    var callbackUrl = Url.Page(
                //    "/OrderBill",
                //    pageHandler: null,
                //    values: new { orderId = model.OrderId },
                //    "http",
                //    "codewarenet-001-site17.dtempurl.com"
                //    );
                //    if (Customer != null)
                //    {
                //        try
                //        {
                //            await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Your Order Bill",
                //      $"Go to your Bill Information page by clicking on this link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Bill Information</a>.");
                //        }

                //        catch (Exception e)
                //        {
                //            return Ok(new { Status = false, Message = e.Message });

                //        }
                //    }
                //    var delivery = _context.Deliveries.Find(model.DeliveryId);
                  
                //    if (delivery != null)
                //    {
                //        try
                //        {
                //            await _emailSender.SendEmailAsync(delivery.Email, "Delivery Order",
                //          $"Go to Order Information to get customer information <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Order Information</a>.");

                //        }
                //        catch (Exception)
                //        {
                //            return RedirectToPage("SomethingwentError");
                //        }
                //    }
                //    return Ok(new { Status = "true", Message = "Thank You", Order = model.OrderItem });
                //}

            }
            return Ok(new { Status = "false", Message= "SomethingwentError" });

        }

        [HttpGet]
        public async Task<IActionResult> Configuration()
        {
            try
            {
                var configuration = _context.Configurations.Select(e => e.WhatsApp).FirstOrDefaultAsync();
                return Ok(new { configuration });
            }
            catch(Exception e)
            {
                return Ok(new { Message = " user Not found" });

            }
            
        }
           //[HttpPost]
           //public async Task<IActionResult> makeOrder([FromBody] Order model)
           //{
           //    foreach (var orderItem in model.OrderItem)
           //    {
           //        var item = _context.Items.Find(orderItem.ItemId);
           //        if (item.Stock == 0 || item.Stock == null)
           //        {
           //            return Ok(new { Message = "Out of Stock", item });

           //        }
           //        if (item.Stock < orderItem.Qty)
           //        {
           //            return Ok(new { Message = " Item Quantity less than Stock Quantity", item });
           //        }

           //        item.Stock -= orderItem.Qty;
           //        var UpdatedItem = _context.Items.Attach(item);
           //        UpdatedItem.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
           //    }
           //    model.OrderDate = DateTime.Now;
           //    var maxserial = Convert.ToInt32(_context.Orders.Max(e => e.OrderSerial));
           //    model.OrderSerial = Convert.ToString(maxserial + 1);
           //    model.Closed = false;


           //    try
           //    {
           //        await _context.SaveChangesAsync();

           //        if (model.CustomerId != null)
           //        {
           //            var carts = _context.Carts.Where(e => e.CustomerId == model.CustomerId);
           //            _context.Carts.RemoveRange(carts);
           //            _context.Orders.Add(model);
           //            _context.SaveChanges();
           //        }

           //    }
           //    catch (Exception)
           //    {
           //        return BadRequest(new { Message = "Something went Error" });
           //    }
           //    var Customer = _context.Customers.Find(model.CustomerId);
           //    var applicationuser = _db.Users.FirstOrDefault(e => e.EntityId == Customer.CustomerId);
           //    var requesturl = "https://api.upayments.com/test-payment";
           //    var fields = new
           //    {
           //        merchant_id = 3309,
           //        username = "TheEquineHospital",
           //        password = "IYHX2COWHFOn",
           //        order_id = model.OrderId,
           //        total_price = model.Total,
           //        test_mode = 0,
           //        CstFName = Customer.CustomerNameEn,
           //        CstEmail = Customer.CustomerEmail,
           //        CstMobile = Customer.CustomerPhone,
           //        api_key = "56af48269041507f6c11f9ff34967af355078bf6",
           //        //api_key= "$2y$10$K8Gredu7YTZZh9jIWebRKes7eXR0TUnZabmXThS9C9jdgGD0xIDv6",
           //        success_url = "http://codewarenet-001-site11.dtempurl.com/privacy",
           //        error_url = "http://codewarenet-001-site11.dtempurl.com/error"
           //    };
           //    var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
           //    var task = httpClient.PostAsync(requesturl, content);
           //    var result = await task.Result.Content.ReadAsStringAsync();

           //    var callbackUrl = Url.Page(
           //        "/OrderBill",
           //        pageHandler: null,
           //        values: new { orderId = model.OrderId },
           //        "http",
           //        "codewarenet-001-site11.dtempurl.com"
           //        );
           //    if (Customer != null)
           //    {

           //        try
           //        {
           //            await _emailSender.SendEmailAsync(Customer.CustomerEmail, "Your Order Bill",
           //      $"Go to your Bill Information page by clicking on this link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Bill Information</a>.");
           //        }

           //        catch (Exception)
           //        {
           //            return BadRequest(new { Message = "Can not Send Email!" });
           //        }
           //    }

           //    return Ok(new { Message = " Order Added" });
           //}

           [HttpPost]
        public async Task<IActionResult> UpdateCart([FromQuery] int cartId, int customerId, int qty)
        {
            var cartModel = await _context.Carts.Where(c => c.CartId == cartId & c.CustomerId == customerId).FirstOrDefaultAsync();
            if (cartModel == null)
            {
                return Ok(new { Message = "  Item Not Found" });
            }
            cartModel.QTY = qty;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Item Updated" });

        }

        [HttpPost]
        public async Task<IActionResult> ReadNotification([FromQuery] int NotificationId, [FromQuery] int CustomerId)
        {

            Notifications NotificationsModel = await _context.Notifications.FirstOrDefaultAsync(c => c.Id == NotificationId & c.CustomerId == CustomerId);

            if (NotificationsModel == null)
            {
                return Ok(new { Message = "  Item Not Found" });
            }

            //set new values

            NotificationsModel.IsReaded = true;


            _context.Entry(NotificationsModel).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Item Updated" });


        }

        [HttpGet]
        public async Task<IActionResult> FaqsList()
        {
            List<FAQ> lstFaqs = new List<FAQ>();
            lstFaqs = await _context.FAQs.ToListAsync();
            return Ok(new { lstFaqs });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPaymentMethod([FromQuery] int userId)
        {
            Order order = await _context.Orders.Where(o => o.CustomerId == userId).FirstOrDefaultAsync();
            if (order == null)
            {
                return Ok(new { Message = "Order Not Found" });
            }
            PaymentMethod paymentMethod = await _context.PaymentMethods.FindAsync(order.PaymentMethodId);
            if (paymentMethod == null)
            {
                return Ok(new { Message = "Payment Method Not Found" });
            }
            return Ok(new { paymentMethod });
        }

        /* [HttpDelete]
         public async Task ClearUserNotification(int key)
         {
             var model = await _context.PageContent.FirstOrDefaultAsync(item => item.PageContentId == key);

             _context.PageContent.Remove(model);
             await _context.SaveChangesAsync();
         }*/

        [HttpGet]
        public async Task<IActionResult> userCart(int id)
        {

            //var sliderList = await _context.HomeSliders.ToListAsync();
            var userCart = await _context.Carts.Where(c => c.CustomerId == id).ToListAsync();
            return Ok(new { userCart });
        }
        //[HttpGet]
        //public async Task<IActionResult> ChangePassword(string password)
        //{


        //    //var sliderList = await _context.HomeSliders.ToListAsync();
        //    var userCart = await _context.Carts.Where(c => c.CustomerId == id).ToListAsync();

        //    return Ok(new { userCart });
        //}

        //[HttpGet]
        //public async Task<IActionResult> userPatment(int id)
        //{

        //    //var sliderList = await _context.HomeSliders.ToListAsync();
        //    var userPatment = await _context.PaymentMethods.Where(c => c.CustomerId == id).ToListAsync();

        //    return Ok(new { userPatment });
        //}
        [HttpPut]
        public async Task<IActionResult> EditCustomer([FromBody] RegistrationModel Model)
        {
            try
            {
                Customer customer = _context.Customers.FirstOrDefault(e => e.CustomerId == Model.CustomerId);
                if (customer == null)
                    return Ok(new { Message = "Customer Not Found" });


                if (Model.CustomerImage != string.Empty)
                {
                    var wwwroot = _hostEnvironment.WebRootPath;
                    var ImagePath = Path.Combine(wwwroot, "Images/Customer/" + customer.CustomerImage);
                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }
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
                if (Model.CustomerAddress != string.Empty)
                {
                    customer.CustomerAddress = Model.CustomerAddress;
                }
                if (Model.CustomerNameAr != string.Empty)
                {
                    customer.CustomerNameAr = Model.CustomerNameAr;
                }
                if (Model.CustomerNameEn != string.Empty)
                {
                    customer.CustomerNameEn = Model.CustomerNameEn;
                }
                if (Model.CustomerPhone != string.Empty)
                {
                    customer.CustomerPhone = Model.CustomerPhone;
                }
                await _context.SaveChangesAsync();

                return Ok(new { customer });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Something went Error" });
            }



        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return Ok(new { Message = "model not Valid" });
            if (resetPasswordModel.ConfirmPassword != resetPasswordModel.NewPassword)
            {
                return Ok(new { Message = "Confirm Password and New Password not matched" });
            }

            var curUser = _userManager.Users.Where(c => c.EntityId == resetPasswordModel.CustomerId).FirstOrDefault();
            if (curUser == null)
            {
                return StatusCode(409, "Object not found");
            }
            var Result = await _userManager.ChangePasswordAsync(curUser, resetPasswordModel.CurrentPassword, resetPasswordModel.NewPassword);
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
        [HttpGet]
        public async Task<IActionResult> getUserById(int id)
        {
            var user = await _context.Customers.FindAsync(id);


            return Ok(new { user });
        }
        [HttpGet]
        public IActionResult GetCategoryBySectionId(int? SectionId)
        {
            if (SectionId != null)
            {
                try
                {
                    var category = _context.Categories.Include(a=>a.Section).Where(i => i.SectionId == SectionId && i.IsActive == true && i.Section.IsActive == true);
                    return Ok(new { category });
                }
                catch (Exception e)
                {

                    return Ok(new { status = false, Message = e.Message });
                }

            }
            else
            {
                return Ok(new { status = false, Message = "Section Id can not be null" });
            }
        }
        [HttpGet]
        public IActionResult GetSectionById(int? SectionId)
        {
            if (SectionId != null)
            {
                try
                {
                    var Section = _context.Sections.Where(a => a.SectionId == SectionId && a.IsActive == true).FirstOrDefault();
                    return Ok(new { Section });
                }
                catch (Exception e)
                {

                    return Ok(new { status = false, Message = e.Message });
                }
            }
            else
            {
                return Ok(new { status = false, Message = "Section Id can not be null" });
            }

        }
        [HttpGet]
        public IActionResult GetAllSections()
        {
            try
            {
                var Sections = _context.Sections.Include(a=>a.Categories).Where(a => a.IsActive == true).ToList();
                return Ok(new { Sections });
            }
            catch (Exception e)
            {

                return Ok(new { status = false, Message = e.Message });
            }

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
                var list = _context.Area.Where(c => c.AreaIsActive == true && c.CityId == cityId && c.City.CityIsActive == true).OrderBy(c => c.AreaOrderIndex).ToList();
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
                var list = _context.City.Where(c => c.CityIsActive == true).OrderBy(c => c.CityOrderIndex).ToList();
                return Ok(new { list });
            }
            catch (Exception)
            {

                return Ok(new { status = false, message = "Something went wrong" });

            }

        }


    }
}


