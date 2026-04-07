# 🛒 TokoSaya - Enterprise E-Commerce Platform

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-blue?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
![Status](https://img.shields.io/badge/Status-Live_on_Somee-success?style=for-the-badge)

**TokoSaya** adalah platform *e-commerce* berbasis web *full-stack* yang dibangun menggunakan ASP.NET Core MVC. Aplikasi ini dirancang untuk mensimulasikan alur transaksi dunia nyata yang sangat relevan dengan ekosistem bisnis lokal, termasuk manajemen keranjang dinamis, otentikasi peran (Admin/Customer), dan alur verifikasi pembayaran manual.

🌐 **[Lihat Live Demo TokoSaya Disini](http://tokosayafarhan.somee.com)**

---

## ✨ Fitur Unggulan (Key Features)

Aplikasi ini tidak hanya menampilkan katalog produk, tetapi memiliki *business logic* yang solid *end-to-end*:

* **🔐 Role-Based Access Control (RBAC):** Sistem otentikasi aman menggunakan ASP.NET Core Identity. Memisahkan hak akses antara Pelanggan biasa dan Administrator toko.
* **🛍️ Dynamic Shopping Cart:** Keranjang belanja interaktif berbasis JavaScript murni dan *Session*, memungkinkan kalkulasi harga *real-time* dan pemilihan spesifik barang (*checkbox*) sebelum *checkout*.
* **💳 Manual Payment Workflow:** Alur *checkout* khusus di mana pelanggan mengunggah foto bukti transfer secara manual, lengkap dengan penanganan *edge-case* (melanjutkan unggahan jika koneksi terputus).
* **📦 Order Management System:** Dasbor Admin komprehensif untuk memantau pesanan masuk, meninjau bukti pembayaran, dan mengubah status pesanan (Pending -> Diproses -> Dikirim -> Selesai).
* **📝 Customer Notes:** Integrasi catatan khusus dari pembeli ("Catatan untuk Penjual") yang terhubung langsung dari halaman *checkout* ke nota pesanan Admin.
* **👤 Account Management:** Halaman profil yang memungkinkan pengguna memperbarui nama, detail akun, dan kata sandi dengan aman.

---

## 🛠️ Teknologi yang Digunakan (Tech Stack)

* **Backend:** C# & ASP.NET Core MVC (.NET 8)
* **Database:** SQLite (untuk portabilitas dan kemudahan *deployment*) & Entity Framework Core (Code-First Approach)
* **Frontend:** Razor Pages, HTML5, CSS3, JavaScript murni, & Bootstrap 5
* **Authentication:** ASP.NET Core Identity
* **Hosting / Deployment:** Somee.com (Windows Server IIS)