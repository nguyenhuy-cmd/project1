using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

class Phim
{
    public string TenPhim { get; set; } = string.Empty;
    public string DaoDien { get; set; } = string.Empty;
    public int NamSanXuat { get; set; }
    public string TheLoai { get; set; } = string.Empty;
    public string DienVien { get; set; } = string.Empty;
    public string QuocGia { get; set; } = string.Empty;
    public string ThoiLuong { get; set; } = string.Empty;
}

class Program
{
     static void DrawColoredBox(int x, int y, int width, int height, ConsoleColor color)
{
    ConsoleColor oldBack = Console.BackgroundColor;
    Console.BackgroundColor = color;
 
    for (int row = 0; row < height; row++)
    {
        for (int col = 0; col < width; col++)
        {
            Console.SetCursorPosition(x + col, y + row);
            Console.Write(" ");
        }
    }
 
    Console.BackgroundColor = oldBack;
}
static void WriteTextInBox(int x, int y, string text, ConsoleColor bgColor, ConsoleColor fgColor = ConsoleColor.White)
{
    ConsoleColor oldBg = Console.BackgroundColor;
    ConsoleColor oldFg = Console.ForegroundColor;
 
    Console.BackgroundColor = bgColor;
    Console.ForegroundColor = fgColor;
 
    for (int i = 0; i < text.Length; i++)
    {
        Console.SetCursorPosition(x + i, y);
        Console.Write(text[i]);
    }
 
    Console.BackgroundColor = oldBg;
    Console.ForegroundColor = oldFg;
}
    static void VeArtMenu()
{
    string[] art = {
        " __  __  _____  _   _  _   _      ____   _    _  _  __  __",
        "|  \\/  || ____|| \\ | || | | |    |  _ \\ | |  | || ||  \\/  |",
        "| |\\/| ||  _|  |  \\| || | | |    | |_) || |__| || || |\\/| |",
        "| |  | || |___ | |\\  || |_| |    |  __/ | |__| || || |  | |",
        "|_|  |_||_____||_| \\_||_____|    |_|    |_|  |_||_||_|  |_|",
    };

    DrawColoredBox(0, 0, Console.WindowWidth, 7, ConsoleColor.Magenta);
    Console.BackgroundColor = ConsoleColor.Magenta;

    for (int i = 0; i < art.Length; i++)
{
    int x = (Console.WindowWidth - art[i].Length) / 2;
    for (int j = 0; j < art[i].Length; j++)
    {
        Console.SetCursorPosition(x + j, i + 1);
        char c = art[i][j];
        Console.ForegroundColor = (c == ' ') ? ConsoleColor.Magenta : ConsoleColor.White;
        Console.Write(c);
    }
}

    Console.ResetColor();
}


    static bool daVeGiaoDien = false; // Đã vẽ giao diện chính chưa
    static int viTriChonCu = -1; // Lưu vị trí chọn cũ để chỉ vẽ lại dòng cũ/mới

    static void VeHopMau(int x, int y, int rong, int cao, ConsoleColor mau)
    {
        ConsoleColor mauCu = Console.BackgroundColor;
        Console.BackgroundColor = mau;
        for (int dong = 0; dong < cao; dong++)
        {
            for (int cot = 0; cot < rong; cot++)
            {
                Console.SetCursorPosition(x + cot, y + dong);
                Console.Write(" ");
            }
        }
        Console.BackgroundColor = mauCu;
    }
    static void GhiChuTrongHop(int x, int y, string chu, ConsoleColor mauNen, ConsoleColor mauChu = ConsoleColor.White)
    {
        ConsoleColor nenCu = Console.BackgroundColor;
        ConsoleColor chuCu = Console.ForegroundColor;
        Console.BackgroundColor = mauNen;
        Console.ForegroundColor = mauChu;
        for (int i = 0; i < chu.Length; i++)
        {
            Console.SetCursorPosition(x + i, y);
            Console.Write(chu[i]);
        }
        Console.BackgroundColor = nenCu;
        Console.ForegroundColor = chuCu;
    }
    static void VeKhungMenuVaPanelMotLan()
    {
        if (daVeGiaoDien) return;
        VeArtMenu();
        int y = 7, lw = Console.WindowWidth / 2, rw = Console.WindowWidth - lw;
        VeHopMau(0, y, lw, 25, ConsoleColor.Yellow);
        VeHopMau(lw, y, rw, 25, ConsoleColor.Cyan);
        Console.SetCursorPosition(2, y + 1);
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("DANH SÁCH PHIM");
        Console.ResetColor();
        VePosterPhim();
        daVeGiaoDien = true;
    }

    static void VeDanhSachPhim(List<Phim> ds, int chon)
    {
        int y = 7, lw = Console.WindowWidth / 2, max = Math.Min(ds.Count, 20);
        for (int i = 0; i < max; i++)
        {
            int row = y + 3 + i;
            var bg = (i % 2 == 0) ? ConsoleColor.White : ConsoleColor.Yellow;
            var fg = (i % 2 == 0) ? ConsoleColor.DarkRed : ConsoleColor.Blue;
            VeHopMau(0, row, lw, 1, bg);
            Console.SetCursorPosition(4, row);
            if (i + 1 == chon)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"> {i + 1}. {ds[i].TenPhim}   ");
            }
            else
            {
                Console.BackgroundColor = bg;
                Console.ForegroundColor = fg;
                Console.Write($"  {i + 1}. {ds[i].TenPhim}   ");
            }
            Console.ResetColor();
        }
        for (int i = max; i < 20; i++) VeHopMau(0, y + 3 + i, lw, 1, (i % 2 == 0) ? ConsoleColor.White : ConsoleColor.Yellow);
    }

    static void VePosterPhim()
    {
        int lw = Console.WindowWidth / 2, px = lw, py = 7, pw = Console.WindowWidth - lw, ph = 25;
        for (int row = 0; row < ph; row++)
        {
            Console.SetCursorPosition(px, py + row);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write(new string(' ', pw));
        }
        string[] poster =
        {
            new string(' ', Math.Max(0, (pw-22)/2)) + "🎬  C I N E M A  🎬",
            new string(' ', Math.Max(0, (pw-28)/2)) + "┌──────────────────────────┐",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│                          │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│        ██████████        │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│        █      █          │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│        █      █          │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│        ██████████        │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "│                          │",
            new string(' ', Math.Max(0, (pw-28)/2)) + "└──────────────────────────┘",
            new string(' ', Math.Max(0, (pw-24)/2)) + "🍿  🎟️  🎫  🍿  🍿  🎟️  🎫  🍿",
            new string(' ', Math.Max(0, (pw-28)/2)) + "════════════════════════════"
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.Cyan;
        for (int i = 0; i < poster.Length; i++)
        {
            if (py + i < Console.WindowHeight) {
                Console.SetCursorPosition(px, py + i);
                Console.Write(poster[i].PadRight(pw));
            }
        }
        for (int i = 0; i < 8; i++)
        {
            int gheCount = Math.Min(10 + i * 2, pw / 2); 
            int ghePad = Math.Max(0, (pw - gheCount * 2) / 2);
            for (int g = 0; g < gheCount; g++)
            {
                int x = px + ghePad + g * 2;
                int y = py + 11 + i;
                if (x >= px && x < px + pw && y < Console.WindowHeight) {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = (g % 2 == 0) ? ConsoleColor.Red : ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write("🪑");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" ");
                }
            }
        }
        Console.ResetColor();
    }

    static void VeChiTietPhim(Phim p, bool datVe)
    {
        int y = 7, lw = Console.WindowWidth / 2, rw = Console.WindowWidth - lw, height = 14;
        VeHopMau(0, y - 1, Console.WindowWidth, 7, ConsoleColor.Magenta);
        VeHopMau(0, y, lw, height, ConsoleColor.Yellow);
        VeHopMau(lw, y, rw, height, ConsoleColor.Cyan);
        string tieuDe = "THÔNG TIN PHIM";
        int tieuDeX = (lw - tieuDe.Length) / 2;
        Console.SetCursorPosition(tieuDeX, y + 1);
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(tieuDe.PadRight(lw - tieuDeX));
        string[] thongTin = {
            $"Tên phim: {p.TenPhim}",
            $"Thể loại: {p.TheLoai}",
            $"Diễn viên: {p.DienVien}",
            $"Đạo diễn: {p.DaoDien}",
            $"Quốc gia: {p.QuocGia}",
            $"Thời lượng: {p.ThoiLuong}",
            $"Năm sản xuất: {p.NamSanXuat}"
        };
        for (int i = 0; i < thongTin.Length; i++)
        {
            int row = y + 3 + i;
            var bg = (i % 2 == 0) ? ConsoleColor.White : ConsoleColor.Yellow;
            var fg = (i % 2 == 0) ? ConsoleColor.DarkRed : ConsoleColor.Blue;
            VeHopMau(0, row, lw, 1, bg);
            int x = (lw - thongTin[i].Length) / 2;
            Console.SetCursorPosition(Math.Max(0, x), row);
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Write(thongTin[i]);
            Console.ResetColor();
        }
        string nut1 = "[Quay lại]", nut2 = "[Đặt vé]";
        int nutY = y + 11;
        int nut1X = lw / 2 - nut1.Length - 2;
        int nut2X = lw / 2 + 2;
        VeHopMau(0, nutY, lw, 1, ConsoleColor.Yellow);
        Console.SetCursorPosition(nut1X, nutY);
        if (!datVe) { Console.BackgroundColor = ConsoleColor.DarkYellow; Console.ForegroundColor = ConsoleColor.White; }
        else { Console.BackgroundColor = ConsoleColor.Yellow; Console.ForegroundColor = ConsoleColor.Blue; }
        Console.Write(nut1);
        Console.ResetColor();
        Console.SetCursorPosition(nut2X, nutY);
        if (datVe) { Console.BackgroundColor = ConsoleColor.DarkYellow; Console.ForegroundColor = ConsoleColor.White; }
        else { Console.BackgroundColor = ConsoleColor.Yellow; Console.ForegroundColor = ConsoleColor.Blue; }
        Console.Write(nut2);
        Console.ResetColor();
        string[] artThongTin = {
            "   ┌──────────────┐   ",
            "   │  🎬  🎥      |    ",
            "   │  THÔNG TIN   │   ",
            "   │    PHIM      │   ",
            "   └──────────────┘   "
        };
        int artWidth = artThongTin[0].Length;
        int artStartX = lw + (rw - artWidth) / 2;
        int artStartY = y + (height - artThongTin.Length) / 2;
        for (int i = 0; i < artThongTin.Length; i++)
        {
            int row = artStartY + i;
            Console.SetCursorPosition(artStartX, row);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(artThongTin[i]);
        }
        Console.ResetColor();
    }

    static void HandleMenuInput(ref int chon, int phimCount, ref bool inMenu)
    {
        var k = Console.ReadKey(true).Key;
        if (inMenu && (k == ConsoleKey.UpArrow || k == ConsoleKey.LeftArrow)) chon = chon > 1 ? chon - 1 : phimCount;
        else if (inMenu && (k == ConsoleKey.DownArrow || k == ConsoleKey.RightArrow)) chon = chon < phimCount ? chon + 1 : 1;
        else if (inMenu && k == ConsoleKey.Enter) inMenu = false;
        else if (!inMenu && k == ConsoleKey.Escape) inMenu = true;
        else if (inMenu && k == ConsoleKey.Escape) { Console.WriteLine("\nKết thúc chương trình."); Environment.Exit(0); }
    }

    static void HandleDetailInput(ref bool datVe, ref bool inMenu)
    {
        var k = Console.ReadKey(true).Key;
        if (k == ConsoleKey.LeftArrow || k == ConsoleKey.RightArrow) datVe = !datVe;
        else if (k == ConsoleKey.Enter) inMenu = true;
        else if (k == ConsoleKey.Escape) inMenu = true;
    }

    static void Main(string[] args)
    {
        Console.Clear();
        KiemTraKetNoiMySQL();
        List<Phim> danhSachPhim = null;
        try
        {
            danhSachPhim = LayDanhSachPhimTuMySQL();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nKhông thể lấy danh sách phim từ MySQL: " + ex.Message);
            Console.ResetColor();
            Console.WriteLine("Nhấn phím bất kỳ để thoát...");
            Console.ReadKey();
            return;
        }
        if (danhSachPhim == null || danhSachPhim.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nKhông có dữ liệu phim hoặc không thể kết nối đến MySQL!");
            Console.ResetColor();
            Console.WriteLine("Nhấn phím bất kỳ để thoát...");
            Console.ReadKey();
            return;
        }
        int chon = 1;
        bool inMenu = true;
        daVeGiaoDien = false;
        viTriChonCu = -1;
        while (true)
        {
            if (inMenu)
            {
                VeKhungMenuVaPanelMotLan();
                if (chon != viTriChonCu)
                {
                    VeDanhSachPhim(danhSachPhim, chon);
                    viTriChonCu = chon;
                }
            }
            HandleMenuInput(ref chon, danhSachPhim.Count, ref inMenu);
            if (!inMenu)
            {
                bool datVeSelected = false;
                Console.Clear(); // Xóa để vào màn chi tiết
                daVeGiaoDien = false; // Để khi quay lại menu sẽ vẽ lại giao diện
                while (!inMenu)
                {
                    VeChiTietPhim(danhSachPhim[chon - 1], datVeSelected);
                    HandleDetailInput(ref datVeSelected, ref inMenu);
                }
                viTriChonCu = -1; // Để khi quay lại menu sẽ vẽ lại dòng chọn
            }
        }
    }
    static void KiemTraKetNoiMySQL()
    {
        string connectionString = "server=localhost;user=root;password=140923;database=phimdb;";
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Kết nối MySQL thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kết nối thất bại: " + ex.Message);
            }
        }
    }
    static List<Phim> LayDanhSachPhimTuMySQL()
    {
        var ds = new List<Phim>();
        string connectionString = "server=localhost;user=root;password=140923;database=phimdb;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new MySqlCommand("SELECT tenphim, daodien, namsanxuat, theloai, dienvien, quocgia, thoiluong FROM phim", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ds.Add(new Phim
                    {
                        TenPhim = reader.GetString(0),
                        DaoDien = reader.GetString(1),
                        NamSanXuat = reader.GetInt32(2),
                        TheLoai = reader.GetString(3),
                        DienVien = reader.GetString(4),
                        QuocGia = reader.GetString(5),
                        ThoiLuong = reader.GetString(6)
                    });
                }
            }
        }
        return ds;
    }
}