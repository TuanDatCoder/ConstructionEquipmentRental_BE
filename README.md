# BuildLease - Nền tảng Thuê và Quản lý Thiết bị Xây dựng

## 🔧 **Giới thiệu**

BuildLease là nền tảng giúp chủ store (người cho thuê) đăng cho thuê các thiết bị xây dựng và người dùng (khách hàng) thuê thiết bị một cách nhanh chóng, dễ dàng. Nền tảng này giống một mạng xã hội, nơi người dùng có thể tương tác và cho đánh giá sản phẩm.

- **Tên dự án**: BuildLease
- **Team thực hiện**: SparkTechVentures

---

## ✨ **Tính năng của hệ thống**

### ⭐ **Guest (Khách không đăng nhập)**

- Xem danh sách sản phẩm, thông tin chi tiết (hình ảnh, giá cả, mô tả, đánh giá...)
- Tìm kiếm và lọc sản phẩm theo danh mục, giá và các tiêu chí khác
- Xem thông tin chi tiết về cửa hàng (tên, địa chỉ, giờ mở cửa...)

### ⭐ **Customer (Khách hàng đã đăng nhập)**

- Đăng ký/đăng nhập tài khoản (hỗ trợ Google/Facebook, JWT Auth)
- Thuê thiết bị trực tuyến (thêm vào giỏ hàng, thanh toán trực tuyến)
- Tích điểm nâng hạng khách hàng (Đồng, Bạc, Vàng, Kim Cương)
- Xem lịch sử đơn hàng, trạng thái và theo dõi vận chuyển
- Đánh giá sản phẩm, viết nhận xét, báo cáo vấn đề đơn hàng

### ⭐ **Staff (Nhân viên cửa hàng)**

- Kiểm tra báo cáo từ khách hàng và chủ store
- Chấp nhận và kiểm tra người cho thuê (Lessor)

### ⭐ **Admin (Quản trị viên)**

- Quản lý người dùng (Khách hàng, Nhân viên, Admin)
- Quản lý sản phẩm (CRUD sản phẩm, danh mục)
- Quản lý đơn hàng (theo dõi, cập nhật trạng thái đơn)
- Quản lý tài chính (doanh thu, chi phí, lợi nhuận theo chi nhánh)
- Xem báo cáo tổng hợp từng chi nhánh

### ⭐ **Lessor (Chủ store)**

- Đăng sản phẩm cho thuê
- Quản lý các sản phẩm đã đăng và đang cho thuê
- Xem báo cáo lợi nhuận của sản phẩm

---

## 🌐 **Công nghệ sử dụng**

- **Back-end**: C#, Microsoft SQL Server
- **Front-end**: ReactJS
- **Mobile**: Flutter
- **Bảo mật**: JWT (JSON Web Token) cho xác thực và phân quyền
- **Real-time**: SignalR để theo dõi đơn hàng và chat hỗ trợ khách hàng
- **Lưu trữ tệp**: Firebase để lưu hình ảnh sản phẩm và hóa đơn
- **Thanh toán**: Hỗ trợ VNPAY, chuyển khoản ngân hàng, ví điện tử

---

## 📘 **Cài đặt và triển khai**

### **Yêu cầu hệ thống**

- **Back-end**: Yêu cầu môi trường .NET Core 8.0 trở lên
- **Front-end**: npm
- **Mobile**: Flutter (Android)
- **Cơ sở dữ liệu**: Microsoft SQL Server

### **Cài đặt môi trường phát triển**

1. **Cài đặt các công cụ cần thiết**
   - Cài đặt .NET SDK từ [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
   - Cài đặt Flutter từ [flutter.dev](https://flutter.dev/)
   
2. **Cài đặt cơ sở dữ liệu**
   - Khởi chạy Microsoft SQL Server
   - Khởi tạo database "BuildLeaseDB"
   
3. **Chạy ứng dụng**
   - Chạy lệnh `dotnet run` trong thư mục back-end
   - Chạy lệnh `npm start` trong thư mục front-end
   - Chạy lệnh `flutter run` trong thư mục mobile

---

## ⚙️ **Kiến trúc hệ thống**

- **API Gateway**: Định tuyến các yêu cầu từ client đến các dịch vụ nội bộ
- **Microservices**: Phân chia các dịch vụ như User Service, Product Service, Order Service để dễ bảo trì và mở rộng
- **Event-Driven**: Sử dụng SignalR và các event bus để xử lý các sự kiện theo thời gian thực
- **CI/CD**: Triển khai tự động với GitHub Actions và Azure Pipelines

---

## 🚀 **Hướng dẫn sử dụng**

1. **Truy cập nền tảng**
   - Với tư cách khách, bạn có thể duyệt sản phẩm mà không cần đăng nhập.
   
2. **Đăng ký tài khoản**
   - Đăng ký tài khoản qua email, Google hoặc Facebook.
   
3. **Thuê sản phẩm**
   - Thêm sản phẩm vào giỏ hàng và tiến hành thanh toán.
   
4. **Theo dõi đơn hàng**
   - Theo dõi trạng thái đơn hàng của bạn từ "Chờ xác nhận" đến "Hoàn tất".

---

## 📚 **Tài liệu bổ sung**

- [Tài liệu API](https://example.com/api-docs)
- [Hướng dẫn cài đặt](https://example.com/setup-guide)

---

## 📞 **Hỗ trợ**

Nếu bạn gặp bất kỳ vấn đề nào hoặc có câu hỏi, vui lòng liên hệ đội ngũ hỗ trợ của chúng tôi qua email: support@sparktechventures.com.

