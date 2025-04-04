using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class ZerdaLiveContext : DbContext
    {
        
        public ZerdaLiveContext(DbContextOptions<ZerdaLiveContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Advertisment> Advertisments { get; set; }
        public virtual DbSet<ApiKey> ApiKeys { get; set; }
        public virtual DbSet<AppImagesDark> AppImagesDarks { get; set; }
        public virtual DbSet<AppImagesLight> AppImagesLights { get; set; }
        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ChannelReport> ChannelReports { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countrys { get; set; }
        public virtual DbSet<DarkColor> DarkColors { get; set; }
        public virtual DbSet<DeviceLogo> DeviceLogos { get; set; }
        public virtual DbSet<DeviceToken> DeviceTokens { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<EpisodeReport> EpisodeReports { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<FilmSnCategory> FilmSnCategories { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Library> Librarys { get; set; }
        public virtual DbSet<LightColor> LightColors { get; set; }
        public virtual DbSet<MoviesReport> MoviesReports { get; set; }
        public virtual DbSet<SactionValue> SactionValues { get; set; }
        public virtual DbSet<ScEvent> ScEvents { get; set; }
        public virtual DbSet<ScMatch> ScMatches { get; set; }
        public virtual DbSet<ScTeam> ScTeams { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\MUHAMMADYASEEN;Database=ZerdaLive;Integrated Security=False;User ID=sa;password=0988905898");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Advertisment>(entity =>
            {
                entity.HasKey(e => e.AdsId);

                entity.ToTable("Advertisment");

                entity.Property(e => e.AdsId).HasColumnName("Ads_ID");

                entity.Property(e => e.AdId)
                    .HasMaxLength(250)
                    .HasColumnName("Ad_ID");

                entity.Property(e => e.AdSaction)
                    .HasMaxLength(30)
                    .HasColumnName("Ad_Saction");
            });

            modelBuilder.Entity<ApiKey>(entity =>
            {
                entity.Property(e => e.ApiKeyId).HasColumnName("ApiKey_ID");

                entity.Property(e => e.ApiKey1)
                    .HasMaxLength(150)
                    .HasColumnName("ApiKey");
            });

            modelBuilder.Entity<AppImagesDark>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK_AppImages");

                entity.ToTable("AppImages_Dark");

                entity.Property(e => e.ImageId).HasColumnName("Image_ID");

                entity.Property(e => e.ImageSaction).HasMaxLength(50);
            });

            modelBuilder.Entity<AppImagesLight>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.ToTable("AppImages_Light");

                entity.Property(e => e.ImageId).HasColumnName("Image_ID");

                entity.Property(e => e.ImageSaction).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatgoryId)
                    .HasName("PK_Catgorys");

                entity.Property(e => e.CatgoryId).HasColumnName("Catgory_ID");

                entity.Property(e => e.CatgoryName)
                    .HasMaxLength(50)
                    .HasColumnName("Catgory_Name");
            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.ChannelId).HasColumnName("Channel_ID");

                entity.Property(e => e.ChannalIcon).HasColumnName("Channal_Icon");

                entity.Property(e => e.ChannelName)
                    .HasMaxLength(50)
                    .HasColumnName("Channel_Name");

                entity.Property(e => e.ChannelUrl).HasColumnName("Channel_Url");

                entity.Property(e => e.IsNew).HasColumnName("Is_New");

                entity.Property(e => e.IsTop).HasColumnName("Is_Top");

                entity.Property(e => e.LanguageId).HasColumnName("language_ID");

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(200)
                    .HasColumnName("User_Agent");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Channels_Categorys");

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.Country)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Channels_Countrys");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Channels_Languages");
            });

            modelBuilder.Entity<ChannelReport>(entity =>
            {
                entity.HasKey(e => e.ChannalReportId);

                entity.ToTable("Channel_Reports");

                entity.Property(e => e.ChannalReportId).HasColumnName("Channal_Report_ID");

                entity.Property(e => e.ChannalId).HasColumnName("Channal_ID");

                entity.Property(e => e.ChannalReportDis)
                    .HasMaxLength(200)
                    .HasColumnName("Channal_Report_Dis");

                entity.Property(e => e.SenderToken).HasColumnName("Sender_Token");

                entity.HasOne(d => d.Channal)
                    .WithMany(p => p.ChannelReports)
                    .HasForeignKey(d => d.ChannalId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Channel_Reports_Channels");

                entity.HasOne(d => d.SenderTokenNavigation)
                    .WithMany(p => p.ChannelReports)
                    .HasForeignKey(d => d.SenderToken)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Channel_Reports_DeviceTokens");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountType).HasMaxLength(50);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.CountryFlag).HasColumnName("Country_Flag");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(90)
                    .HasColumnName("Country_Name");
            });

            modelBuilder.Entity<DarkColor>(entity =>
            {
                entity.HasKey(e => e.ControllId);

                entity.Property(e => e.ControllId).HasColumnName("Controll_ID");

                entity.Property(e => e.ControllColor)
                    .HasMaxLength(50)
                    .HasColumnName("Controll_Color");

                entity.Property(e => e.ControllName)
                    .HasMaxLength(50)
                    .HasColumnName("Controll_Name");
            });

            modelBuilder.Entity<DeviceLogo>(entity =>
            {
                entity.ToTable("DeviceLogo");

                entity.Property(e => e.DeviceLogoId).HasColumnName("DeviceLogo_ID");

                entity.Property(e => e.DeviceLogoDate)
                    .HasColumnType("datetime")
                    .HasColumnName("DeviceLogo_Date");

                entity.Property(e => e.DeviceTokenId).HasColumnName("Device_Token_ID");

                entity.HasOne(d => d.DeviceToken)
                    .WithMany(p => p.DeviceLogos)
                    .HasForeignKey(d => d.DeviceTokenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DeviceLogo_DeviceTokens");
            });

            modelBuilder.Entity<DeviceToken>(entity =>
            {
                entity.Property(e => e.DeviceTokenId).HasColumnName("Device_Token_ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Creation_Date");

                entity.Property(e => e.DeviceToken1)
                    .IsUnicode(false)
                    .HasColumnName("Device_Token");
            });

            modelBuilder.Entity<Episode>(entity =>
            {
                entity.HasKey(e => e.EpisodesId);

                entity.Property(e => e.EpisodesId).HasColumnName("Episodes_ID");

                entity.Property(e => e.EpisodesName)
                    .HasMaxLength(100)
                    .HasColumnName("Episodes_Name");

                entity.Property(e => e.EpisodesUrl).HasColumnName("Episodes_Url");

                entity.Property(e => e.SeasoneId).HasColumnName("Seasone_ID");

                entity.HasOne(d => d.Seasone)
                    .WithMany(p => p.Episodes)
                    .HasForeignKey(d => d.SeasoneId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Episodes_Seasons");
            });

            modelBuilder.Entity<EpisodeReport>(entity =>
            {
                entity.ToTable("Episode_Reports");

                entity.Property(e => e.EpisodeReportId).HasColumnName("Episode_Report_ID");

                entity.Property(e => e.EpisodeId).HasColumnName("Episode_ID");

                entity.Property(e => e.ReportDis)
                    .HasMaxLength(250)
                    .HasColumnName("Report_Dis");

                entity.Property(e => e.SenderTokenId).HasColumnName("SenderToken_ID");

                entity.HasOne(d => d.Episode)
                    .WithMany(p => p.EpisodeReports)
                    .HasForeignKey(d => d.EpisodeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Episode_Reports_Episodes");

                entity.HasOne(d => d.SenderToken)
                    .WithMany(p => p.EpisodeReports)
                    .HasForeignKey(d => d.SenderTokenId)
                    .HasConstraintName("FK_Episode_Reports_DeviceTokens");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(e => e.FilmId).HasColumnName("Film_ID");

                entity.Property(e => e.FilmCat).HasColumnName("Film_Cat");

                entity.Property(e => e.FilmCountry).HasColumnName("Film_Country");

                entity.Property(e => e.FilmDis)
                    .HasMaxLength(500)
                    .HasColumnName("Film_Dis");

                entity.Property(e => e.FilmDuration)
                    .HasMaxLength(50)
                    .HasColumnName("Film_Duration");

                entity.Property(e => e.FilmImage).HasColumnName("Film_Image");

                entity.Property(e => e.FilmLang).HasColumnName("Film_Lang");

                entity.Property(e => e.FilmUrl).HasColumnName("Film_Url");

                entity.Property(e => e.FilmeName)
                    .HasMaxLength(100)
                    .HasColumnName("Filme_Name");

                entity.HasOne(d => d.FilmCatNavigation)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.FilmCat)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Films_Film_SN_Category");

                entity.HasOne(d => d.FilmLangNavigation)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.FilmLang)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Films_Languages");
            });

            modelBuilder.Entity<FilmSnCategory>(entity =>
            {
                entity.HasKey(e => e.CatId);

                entity.ToTable("Film_SN_Category");

                entity.Property(e => e.CatId).HasColumnName("Cat_ID");

                entity.Property(e => e.CatName)
                    .HasMaxLength(100)
                    .HasColumnName("Cat_Name");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("History");

                entity.Property(e => e.HistoryId).HasColumnName("History_ID");

                entity.Property(e => e.HistoryName)
                    .HasMaxLength(50)
                    .HasColumnName("History_Name");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.LangId);

                entity.Property(e => e.LangId).HasColumnName("Lang_ID");

                entity.Property(e => e.LangName)
                    .HasMaxLength(50)
                    .HasColumnName("Lang_Name");
            });

            modelBuilder.Entity<Library>(entity =>
            {
                entity.Property(e => e.LibraryId).HasColumnName("Library_ID");

                entity.Property(e => e.LibraryName)
                    .HasMaxLength(50)
                    .HasColumnName("Library_Name");
            });

            modelBuilder.Entity<LightColor>(entity =>
            {
                entity.HasKey(e => e.ControllId);

                entity.Property(e => e.ControllId).HasColumnName("Controll_ID");

                entity.Property(e => e.ControllColor)
                    .HasMaxLength(50)
                    .HasColumnName("Controll_Color");

                entity.Property(e => e.ControllName)
                    .HasMaxLength(50)
                    .HasColumnName("Controll_Name");
            });

            modelBuilder.Entity<MoviesReport>(entity =>
            {
                entity.HasKey(e => e.MovieReportId);

                entity.ToTable("Movies_Reports");

                entity.Property(e => e.MovieReportId).HasColumnName("Movie_Report_ID");

                entity.Property(e => e.MovieId).HasColumnName("Movie_ID");

                entity.Property(e => e.ReportDis)
                    .HasMaxLength(250)
                    .HasColumnName("Report_Dis");

                entity.Property(e => e.SenderTokenId).HasColumnName("SenderToken_ID");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MoviesReports)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Movies_Reports_Films");

                entity.HasOne(d => d.SenderToken)
                    .WithMany(p => p.MoviesReports)
                    .HasForeignKey(d => d.SenderTokenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Movies_Reports_DeviceTokens");
            });

            modelBuilder.Entity<SactionValue>(entity =>
            {
                entity.HasKey(e => e.SactionId);

                entity.Property(e => e.SactionId).HasColumnName("Saction_ID");

                entity.Property(e => e.SactionName)
                    .HasMaxLength(50)
                    .HasColumnName("Saction_Name");

                entity.Property(e => e.SactionValue1).HasColumnName("Saction_Value");
            });

            modelBuilder.Entity<ScEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("Sc_Events");

                entity.Property(e => e.EventId).HasColumnName("Event_ID");

                entity.Property(e => e.EventLogo).HasColumnName("Event_Logo");

                entity.Property(e => e.EventName)
                    .HasMaxLength(50)
                    .HasColumnName("Event_Name");
            });

            modelBuilder.Entity<ScMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId);

                entity.ToTable("Sc_Matches");

                entity.Property(e => e.MatchId).HasColumnName("Match_ID");

                entity.Property(e => e.ChannelCategory).HasColumnName("Channel_Category");

                entity.Property(e => e.Commentator).HasMaxLength(100);

                entity.Property(e => e.EventId).HasColumnName("Event_ID");

                entity.Property(e => e.FirstTeam).HasColumnName("First_Team");

                entity.Property(e => e.FirstTeamGoals).HasColumnName("FirstTeam_Goals");

                entity.Property(e => e.MatchDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Match_Date");

                entity.Property(e => e.SecondTeam).HasColumnName("Second_Team");

                entity.Property(e => e.SecondTeamGoals).HasColumnName("SecondTeam_Goals");

                entity.HasOne(d => d.ChannelCategoryNavigation)
                    .WithMany(p => p.ScMatches)
                    .HasForeignKey(d => d.ChannelCategory)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Sc_Matches_Categorys");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.ScMatches)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Sc_Matches_Sc_Events");

                entity.HasOne(d => d.FirstTeamNavigation)
                    .WithMany(p => p.ScMatchFirstTeamNavigations)
                    .HasForeignKey(d => d.FirstTeam)
                    .HasConstraintName("FK_Sc_Matches_Sc_Teams");

                entity.HasOne(d => d.SecondTeamNavigation)
                    .WithMany(p => p.ScMatchSecondTeamNavigations)
                    .HasForeignKey(d => d.SecondTeam)
                    .HasConstraintName("FK_Sc_Matches_Sc_Teams1");
            });

            modelBuilder.Entity<ScTeam>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.ToTable("Sc_Teams");

                entity.Property(e => e.TeamId).HasColumnName("Team_ID");

                entity.Property(e => e.TeamLogo).HasColumnName("Team_Logo");

                entity.Property(e => e.TeamName)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Name");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(e => e.SeasonsId);

                entity.Property(e => e.SeasonsId).HasColumnName("Seasons_ID");

                entity.Property(e => e.SeasonsDis)
                    .HasMaxLength(150)
                    .HasColumnName("Seasons_Dis");

                entity.Property(e => e.SeasonsImage).HasColumnName("Seasons_Image");

                entity.Property(e => e.SeasonsName)
                    .HasMaxLength(100)
                    .HasColumnName("Seasons_Name");

                entity.Property(e => e.SeriesId).HasColumnName("Series_ID");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(d => d.SeriesId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Seasons_Series");
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.Property(e => e.SeriesId).HasColumnName("Series_ID");

                entity.Property(e => e.SeriesCat).HasColumnName("Series_Cat");

                entity.Property(e => e.SeriesCountry).HasColumnName("Series_Country");

                entity.Property(e => e.SeriesDis)
                    .HasMaxLength(150)
                    .HasColumnName("Series_Dis");

                entity.Property(e => e.SeriesImage).HasColumnName("Series_Image");

                entity.Property(e => e.SeriesLang).HasColumnName("Series_Lang");

                entity.Property(e => e.SeriesName)
                    .HasMaxLength(100)
                    .HasColumnName("Series_Name");

                entity.HasOne(d => d.SeriesCatNavigation)
                    .WithMany(p => p.Series)
                    .HasForeignKey(d => d.SeriesCat)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Series_Film_SN_Category");

                entity.HasOne(d => d.SeriesLangNavigation)
                    .WithMany(p => p.Series)
                    .HasForeignKey(d => d.SeriesLang)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Series_Languages");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Password).HasMaxLength(70);

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .HasMaxLength(70)
                    .HasColumnName("User_Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
