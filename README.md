# GymHub-SoftUni-CSharp-WebProject
## My SoftUni web project written on C# and Asp.Net Core 
**GymHub** is my ASP.NET Core MVC web application project for **SoftUni ASP.NET Core - октомври 2020г.**

## :pencil: Description
**GymHub** is a basic shoppings website which sells all kinds of gym equipment. It has products with categories and a product cart.
            It also has user reviews, comments and ratings for each product. There are also popup notifications for certain actions.
- As a user, you can buy products, leave reviews, comments and ratings for a product. You can see details for your sales from your profile at any time and make a pdf copy of the invoice for your order. You can order and filter your sales and products in the shop via the available option. You can pay for products in 4 different methods(of which 2-Cash On Delivery and Credit or Debit Card, actually work). The Credit or Debit Card payment is done via the PaymentIntents API and Checkout API and page in Stripe. You can also write a message directly to and administrator of the support team via the contacts tab.  
- As an admin you can edit, add and remove categories and products. You can also do everything a normal user can as well. You can also manage the sales and change their statuses. You can refund sales, confirm sales(which in the case of payments with Credit or Debit Card officially charges the customer and withholds funds from them). You can also answer people's questions in the contact tab. You also have access to the Hangfire Dashboard to monitor all jobs activity.
- Hangfire is also used passively during the app's lifetime. There are 2 jobs. One job changes the 3 featured products on the front page every 1 minute. It does so in such a way that every product gets featured randomly, but yet only once per whole rotation. After all the products have been featured the rotation starts again.
- The other use for Hangfire is to delete the unused pictures from Azure blob storage every 5 minutes(which is definitely overkill and it is for the sake of demo and for testing). The need to do that arises from the fact that via playing around with editing the products you can end up with unused images in the blob storage. So to free up space and not clog the storage(impossible to do with an web app on such a small scale but anyway...), and also to get some practice with hangfire, I decided to periodically garbage collect the pictures.
- SignalR is used for the contacts chat page to ensure two way communication between client and server.
- Sendgrid is used to send comment removal justification from the admin to the user when they remove their comment.
- Automapper fuking sucks. I hate that shit. For me it never works and it takes me much more time to configure automapper to work correctly than map the fuking object myself. So fuck Automapper. I used it still for a little bit but yeah...
- Partial views are used a fuckton for many different things.
- View components are used 3 times. To notify the user of their current products count in the cart, to notify them of their unseed messages, to change their profile icon based on their gender.
- Also I wrote my own custom pagination which, dare I say, works pretty wonderfully. You insert one script, use one partial view and in another script tag just call one function with a parameter which defines the name for the queryParam which indicates the current page. For example in the productPage the queryParam was commentsPage(I believe anyway, not 100% sure) and for the Shop page it was productsPage(again, I believe, but not 100% sure).
- For the productPage especially I wrote a metric fuk ton of javascript. Which reminded me why I somewhat despise that language.
<hr/>

## Link to the site 
- https://gymhubweb.azurewebsites.net
## Admin account: 
- Username: adminRosen
- Password: adminRosen
<hr/>

## :hammer: Built With

- Visual Studio Enterprise
- .NET 5.0           
- ASP.NET CORE 5.0
- EF Core 5.0
- ASP.NET CORE views and partial views and sections and view components
- ASP.NET CORE areas
- ASP.NET CORE MVC and Razor Pages
- SQL Server and Microsoft SQL Server Management Studio
- Custom database seeder 
- Custom pagination(that I am actually quite proud of)
- Bootstrap 3.0/4.0 depending on the page
- Javascript (A LOT OF FUKING JAVASCRIPT)
- AJAX real-time Requests
- JQuery and any kind of jQuery plugins such as DataTables and jquery animations
- Lots of boostrap themes and templates
- Stripe Checkout and PaymentIntents API
- Mapbox for maps
- Azure blob storage
- Automapper 
- SendGrid
- Azure App Service + Azure SQL Database + Azure Application Insights
- Hangfire
- SignalR

<h2>Interface</h2>
<hr/>

### - Normal User
![HomePage.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/HomePage.png)

![LoginPage.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/LoginPage.PNG)

![RegisterPage.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/RegisterPage.PNG)

![IdentityProfile.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/IdentityProfile.PNG)

![ShopPage.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ShopPage.PNG)

![ShopPage(Sorted by rating ascending).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ShopPage(Sorted%20by%20rating%20ascending).PNG)

![ShopPage(Sorted by price descending and filtered by categories).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ShopPage(Sorted%20by%20price%20descending%20and%20filtered%20by%20categories).PNG)

![ProductPage1.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ProductPage1.PNG)

![ProductPage2.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ProductPage2.PNG)

![ProductPage(Adding a review).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ProductPage(Adding%20a%20review).PNG)

![ProductPage(Removing another person's comment as admin, and also example of my own hand written pagination).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ProductPage(Removing%20another%20person's%20comment%20as%20admin%2C%20and%20also%20example%20of%20my%20own%20hand%20written%20pagination).PNG)

![Cart.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Cart.PNG)

![CheckoutPage.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/CheckoutPage.png)

![StripePaymentPage.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/StripePaymentPage.PNG)

![AfterPurchase.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AfterPurchase.PNG)

![AllMySales(regular user).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AllMySales(regular%20user).PNG)

![SaleDetails.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/SaleDetails.png)

### - Admin User

![AdminControls.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AdminControls.PNG)

![HangfireDashboard.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/HangfireDashboard.PNG)

![AddProduct2.1.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AddProduct2.1.png)

![AddProduct2.2.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AddProduct2.2.png)

![EditProduct2.1.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/EditProduct2.1.png)

![EditProduct2.2.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/EditProduct2.2.png)

![CategoriesAll.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/CategoriesAll.PNG)

![Create-Edit Category.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Create-Edit%20Category.PNG)

![AllMySales(admin).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AllMySales(admin).PNG)

![AllSales(Admin only).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/AllSales(Admin%20only).PNG)

![ChangeSaleStatus(Admin).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ChangeSaleStatus(Admin).PNG)

![ChangeSaleStatusModal.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ChangeSaleStatusModal.PNG)

![RefundModal.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/RefundModal.PNG)

### - Contacts

![Contacts(Normal User).PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Contacts(Normal%20User).PNG)

![Contacts(Admin).1.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Contacts(Admin).1.PNG)

![Contacts(Admin).2.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Contacts(Admin).2.PNG)

![ReceiveMessageWhileScroledUpInTheChat.PNG](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/ReceiveMessageWhileScroledUpInTheChat.PNG)

License This project is licensed under the MIT License - see the LICENSE.md file for details

## - Database Diagram
![DatabaseDiagram.png](https://github.com/rosenkolev1/GymHub-SoftUni-CSharp-WebProject/blob/main/GymHubUserInterface/Database%20diagram/DatabaseDiagram.png)
