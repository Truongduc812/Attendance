# Template isoft_be_api

## Thông tin chung

updateing ...

# Hướng dẫn cài đặt và khởi tạo dự án từ template.

## Cài đặt template

1. Clone source về máy tính.

2. Mở terminal và di chuyển tới thư mục chứa template của bạn.

3. Chạy lệnh sau để cài đặt template:

4. ```shell
   $ dotnet new install .\
   
   The following template packages will be installed:
      D:\source\path\SourceBaseBE
   
   Success: D:\source\path\SourceBaseBE installed the following templates:
   Template Name                      Short Name    Language  Tags
   ---------------------------------  ------------  --------  -----------------------------
   .NET 7 A template for backend api  isoft_be_api  [C#]      Web/API/Microservices/Backend
   ```

   Lưu ý: Chỉ cần cài đặt 1 lần và dùng lại không cần cài đặt thêm.

### Sử Dụng Template:

1. Mở terminal và di chuyển tới thư mục mà bạn muốn tạo dự án mới.

2. Chạy lệnh sau để tạo dự án từ template:
   
   ```
   $ dotnet new isoft_be_api -n ten_du_an
   ```
   
   Trong đó, `isoft_be_api` là `shortName` của template, và `ten_du_an` là tên bạn muốn đặt cho dự án của mình.

3. Sau khi chạy lệnh trên, dự án sẽ được tạo trong thư mục hiện tại với tên đã chỉ định.

4. Bạn có thể sử dụng lệnh `dotnet run` để chạy dự án và kiểm tra kết quả.

### Gỡ Cài Đặt Template (Tùy Chỉnh):

1. Mở terminal và di chuyển tới thư mục chứa template của bạn.

2. Chạy lệnh sau để gỡ cài đặt template:
   
   ```shell
   dotnet new uninstall .\`
   ```

Lưu ý rằng các thay đổi được thực hiện trong dự án sẽ tuân theo các tham số được định nghĩa trong `template.json`, như `author`, `description`, `namespace`, và `version`.