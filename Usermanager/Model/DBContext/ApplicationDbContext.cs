using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text.Json;
using Usermanager.Model.Entity;

namespace Usermanager.Model.DBContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Province> Provinces { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<Department> Departments { get; set; } = null!;

    public DbSet<Group> Groups { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("People");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.PhoneNumber)
                  .IsRequired()
                  .HasMaxLength(11);

            entity.Property(p => p.PersonnelNumber)
                  .IsRequired()
                  .HasMaxLength(20);


            entity.HasOne(u => u.Group)
                  .WithMany(p => p.Users)
                  .HasForeignKey(u => u.GroupID)
                  .OnDelete(DeleteBehavior.Restrict);


        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .HasMaxLength(100);

            entity.HasMany(d => d.Cities)
                  .WithOne(g => g.Province)
                  .HasForeignKey(g => g.ProvinceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .HasMaxLength(100);

            entity.HasMany(d => d.Users)
                  .WithOne(g => g.City)
                  .HasForeignKey(g => g.CityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(d => d.Groups)
                  .WithOne(g => g.Department)
                  .HasForeignKey(g => g.DepartmentID)
                  .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(e => e.Accessibility)
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                      v => JsonSerializer.Deserialize<List<UserAccess>>(v, (JsonSerializerOptions)null),
                      new ValueComparer<List<UserAccess>>(
                          (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                          c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                          c => c != null ? c.ToList() : null
                      ))
                  .HasColumnType("nvarchar(max)");

            entity.HasIndex(r => r.Name).IsUnique(); 
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Name)
                  .IsRequired()
                  .HasMaxLength(100);


            entity.HasMany(g => g.Roles)
                  .WithMany(d => d.Groups);

        });

        var provinces = new List<Province>
        {
                new Province { Id = 1,  Name = "تهران" },
                new Province { Id = 2,  Name = "البرز" },
                new Province { Id = 3,  Name = "اصفهان" },
                new Province { Id = 4,  Name = "فارس" },
                new Province { Id = 5,  Name = "خراسان رضوی" },
                new Province { Id = 6,  Name = "آذربایجان شرقی" },
                new Province { Id = 7,  Name = "آذربایجان غربی" },
                new Province { Id = 8,  Name = "گیلان" },
                new Province { Id = 9,  Name = "مازندران" },
                new Province { Id = 10, Name = "کرمان" },
                new Province { Id = 11, Name = "هرمزگان" },
                new Province { Id = 12, Name = "خوزستان" },
                new Province { Id = 13, Name = "کردستان" },
                new Province { Id = 14, Name = "کرمانشاه" },
                new Province { Id = 15, Name = "یزد" },
                new Province { Id = 16, Name = "زنجان" },
                new Province { Id = 17, Name = "همدان" },
                new Province { Id = 18, Name = "قزوین" },
                new Province { Id = 19, Name = "قم" },
                new Province { Id = 20, Name = "سمنان" },
                new Province { Id = 21, Name = "ایلام" },
                new Province { Id = 22, Name = "کهگیلویه و بویراحمد" },
                new Province { Id = 23, Name = "چهارمحال و بختیاری" },
                new Province { Id = 24, Name = "لرستان" },
                new Province { Id = 25, Name = "مرکزی" },
                new Province { Id = 26, Name = "بوشهر" },
                new Province { Id = 27, Name = "سیستان و بلوچستان" },
                new Province { Id = 28, Name = "گلستان" }
        };
        modelBuilder.Entity<Province>().HasData(provinces);

        var cities = new List<City>();
        int cityId = 1;

        void AddCities(int provinceId, params string[] names)
        {
            foreach (var n in names)
            {
                cities.Add(new City { Id = cityId++, ProvinceId = provinceId, Name = n });
            }
        }

        AddCities(1, "تهران", "ری", "دماوند", "پردیس", "ورامین", "شهریار", "اسلام‌شهر", "ملارد", "پاکدشت");
        AddCities(2, "کرج", "نظرآباد", "طالقان", "اشتهارد", "فردیس");
        AddCities(3, "اصفهان", "کاشان", "خمینی‌شهر", "نجف‌آباد", "شهرضا", "فلاورجان", "مبارکه");
        AddCities(4, "شیراز", "مرودشت", "جهرم", "لار", "فسا", "کازرون", "داراب");
        AddCities(5, "مشهد", "نیشابور", "سبزوار", "تربت‌حیدریه", "قوچان", "چناران");
        AddCities(6, "تبریز", "مراغه", "مرند", "میانه", "اهر", "بناب");
        AddCities(7, "ارومیه", "خوی", "بوکان", "مهاباد", "سلماس", "میاندوآب");
        AddCities(8, "رشت", "لاهیجان", "انزلی", "آستارا", "تالش", "صومعه‌سرا");
        AddCities(9, "ساری", "بابل", "آمل", "نور", "تنکابن", "قائم‌شهر");
        AddCities(10, "کرمان", "رفسنجان", "جیرفت", "سیرجان", "بم");
        AddCities(11, "بندرعباس", "قشم", "بندر لنگه", "میناب", "حاجی‌آباد");
        AddCities(12, "اهواز", "آبادان", "خرمشهر", "دزفول", "ماهشهر", "شادگان");
        AddCities(13, "سنندج", "سقز", "بانه", "بیجار", "قروه");
        AddCities(14, "کرمانشاه", "اسلام‌آباد غرب", "سرپل ذهاب", "قصر شیرین", "سنقر");
        AddCities(15, "یزد", "میبد", "اردکان", "ابرکوه", "تفت");
        AddCities(16, "زنجان", "ابهر", "خرمدره", "طارم", "ماه‌نشان");
        AddCities(17, "همدان", "ملایر", "نهاوند", "تویسرکان", "رزن");
        AddCities(18, "قزوین", "الوند", "آبیک", "تاکستان");
        AddCities(19, "قم");
        AddCities(20, "سمنان", "شاهرود", "دامغان", "گرمسار");
        AddCities(21, "ایلام", "دهلران", "مهران", "آبدانان");
        AddCities(22, "یاسوج", "گچساران", "دهدشت");
        AddCities(23, "شهرکرد", "فارسان", "بروجن", "لردگان");
        AddCities(24, "خرم‌آباد", "بروجرد", "دورود", "الیگودرز");
        AddCities(25, "اراک", "ساوه", "خمین", "محلات");
        AddCities(26, "بوشهر", "برازجان", "جم", "دیر", "کنگان");
        AddCities(27, "زاهدان", "چابهار", "ایرانشهر", "خاش");
        AddCities(28, "گرگان", "گنبد", "علی‌آباد", "آق‌قلا", "کلاله");

        modelBuilder.Entity<City>().HasData(cities);
<<<<<<< HEAD
=======


        
        var users = new List<User>
    {
        new User
        {
            Id = 1,
            FirstName = "علی",
            LastName = "رضایی",
            PhoneNumber = "09123456789",
            PersonnelNumber = "EMP1001",
            ProvinceId = 1,  // تهران
            CityId = 1      // تهران
        },
        new User
        {
            Id = 2,
            FirstName = "فاطمه",
            LastName = "محمدی",
            PhoneNumber = "09129876543",
            PersonnelNumber = "EMP1002",
            ProvinceId = 2,  // البرز
            CityId = 2       // کرج
        },
        new User
        {
            Id = 3,
            FirstName = "رضا",
            LastName = "کریمی",
            PhoneNumber = "09151234567",
            PersonnelNumber = "EMP1003",
            ProvinceId = 3,  // اصفهان
            CityId = 3       // اصفهان
        },
        new User
        {
            Id = 4,
            FirstName = "سارا",
            LastName = "نجفی",
            PhoneNumber = "09351234567",
            PersonnelNumber = "EMP1004",
            ProvinceId = 4,  // فارس
            CityId = 4       // شیراز
        },
        new User
        {
            Id = 5,
            FirstName = "محمد",
            LastName = "حسینی",
            PhoneNumber = "09161234567",
            PersonnelNumber = "EMP1005",
            ProvinceId = 5,  // خراسان رضوی
            CityId = 5       // مشهد
        }
       };

        modelBuilder.Entity<User>().HasData(users);
>>>>>>> f5cd38e14d9206e34a930935935d92bf3a714736
    }
}

