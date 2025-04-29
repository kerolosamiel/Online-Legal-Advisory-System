using ELawyer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELawyer.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Lawyer> Lawyers { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<SubSpecialization> SubSpecializations { get; set; }
    public DbSet<LawyerSpecialization> LawyerSpecializations { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Response> Responses { get; set; }

    public DbSet<Consultation> Consultations { get; set; }

    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>()
            .HasOne(s => s.Lawyer)
            .WithMany(l => l.Services)
            .HasForeignKey(s => s.LawyerId)
            .OnDelete(DeleteBehavior.Restrict);

        //create composite key
        modelBuilder.Entity<LawyerSpecialization>()
            .HasKey(e => new { e.LawyerId, e.SpecializationId });
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Specialization>().HasData(
            new Specialization
            {
                ID = 1, Name = "Criminal Law",
                Description =
                    "Deals with crimes and legal punishments, representing individuals or the state in cases involving offenses like theft, assault, or murder."
            },
            new Specialization
            {
                ID = 2, Name = "Commercial And Corporate Law",
                Description =
                    "Focuses on business transactions, corporate governance, contracts, mergers, and regulatory compliance to ensure legal and ethical business operations."
            },
            new Specialization
            {
                ID = 3, Name = "Labor And Insurance Law",
                Description =
                    "Covers workplace rights, employment disputes, social security, and insurance claims, ensuring fair treatment and compliance with labor regulations"
            },
            new Specialization
            {
                ID = 4, Name = "Personal Atatus Law",
                Description =
                    " Governs family matters such as marriage, divorce, child custody, and inheritance, ensuring legal protection of personal and familial rights."
            },
            new Specialization
            {
                ID = 5, Name = "Real Estate And Property Law",
                Description =
                    "Regulates property ownership, transactions, leasing, zoning, and land disputes, ensuring legal compliance in real estate dealings."
            },
            new Specialization
            {
                ID = 6, Name = "Administrative Law And Government Issues",
                Description =
                    "Deals with regulations, government agencies, public administration, and legal disputes involving governmental decisions and policies."
            },
            new Specialization
            {
                ID = 7, Name = "International Law And Arbitration",
                Description =
                    "Governs legal relations between nations, international treaties, trade laws, and dispute resolution through arbitration rather than litigation."
            },
            new Specialization
            {
                ID = 8, Name = "Tax Law and Financial Consultation",
                Description =
                    "Covers taxation regulations, compliance, tax planning, and financial advisory services to help individuals and businesses manage their fiscal responsibilities."
            },
            new Specialization
            {
                ID = 9, Name = "Intellectual Property And Copyright Law",
                Description =
                    "Protects creations like inventions, trademarks, patents, and artistic works, ensuring exclusive rights and preventing unauthorized use."
            }
        );
        modelBuilder.Entity<SubSpecialization>().HasData(
            new SubSpecialization { ID = 1, Name = "Defending defendants in criminal cases.", SpecializationID = 1 },
            new SubSpecialization
                { ID = 2, Name = "Providing advice on criminal laws and penalties", SpecializationID = 1 },
            new SubSpecialization
                { ID = 3, Name = "Drug-related issues, theft, murder, fraud, and cybercrimes", SpecializationID = 1 },
            new SubSpecialization
                { ID = 4, Name = "Filing appeals against criminal convictions", SpecializationID = 1 },
            new SubSpecialization
                { ID = 5, Name = "Establishing companies and preparing legal contracts", SpecializationID = 2 },
            new SubSpecialization
            {
                ID = 6, Name = "Drafting and reviewing partnership and intellectual property contracts",
                SpecializationID = 2
            },
            new SubSpecialization { ID = 7, Name = "Bankruptcy and liquidation issues", SpecializationID = 2 },
            new SubSpecialization
                { ID = 8, Name = "Commercial disputes between companies or individuals", SpecializationID = 2 },
            new SubSpecialization { ID = 9, Name = "Dyeing and reviewing employment contracts", SpecializationID = 3 },
            new SubSpecialization
                { ID = 10, Name = "Arbitrary dismissal and employee harassment cases", SpecializationID = 3 },
            new SubSpecialization
                { ID = 11, Name = "Consultations on social insurance and pension laws", SpecializationID = 3 },
            new SubSpecialization
                { ID = 12, Name = "Labor disputes between companies and employees", SpecializationID = 3 },
            new SubSpecialization { ID = 13, Name = "Wedding, divorce and pollen gifts", SpecializationID = 4 },
            new SubSpecialization { ID = 14, Name = "Alimony and child custody", SpecializationID = 4 },
            new SubSpecialization
                { ID = 15, Name = "Providing inheritance according to Sharia and civil laws", SpecializationID = 4 },
            new SubSpecialization
            {
                ID = 16, Name = "Consultations regarding civil marriage and customary marriage", SpecializationID = 4
            },
            new SubSpecialization
                { ID = 17, Name = "Drafting and reviewing sales and lease contracts", SpecializationID = 5 },
            new SubSpecialization { ID = 18, Name = "Disputes between buyers and sellers", SpecializationID = 5 },
            new SubSpecialization
            {
                ID = 19, Name = "Consultations on real estate ownership and legal registration", SpecializationID = 5
            },
            new SubSpecialization
            {
                ID = 20, Name = "Disputes related to residential associations and real estate development",
                SpecializationID = 5
            },
            new SubSpecialization
                { ID = 21, Name = "Issues related to dealing with government agencies", SpecializationID = 6 },
            new SubSpecialization { ID = 22, Name = "Appeals informed administrative decisions", SpecializationID = 6 },
            new SubSpecialization
                { ID = 23, Name = "onsultations regarding legal licenses and permits", SpecializationID = 6 },
            new SubSpecialization
                { ID = 24, Name = "Issues related to treaties and international relations", SpecializationID = 7 },
            new SubSpecialization
            {
                ID = 25, Name = "Legal consultations on international trade and foreign investment",
                SpecializationID = 7
            },
            new SubSpecialization
                { ID = 26, Name = "International dispute resolution and commercial arbitration", SpecializationID = 7 },
            new SubSpecialization
            {
                ID = 27, Name = "Consultations on tax obligations for individuals and companies", SpecializationID = 8
            },
            new SubSpecialization
                { ID = 28, Name = "Tax evasion issues and legal accounting procedures", SpecializationID = 8 },
            new SubSpecialization
                { ID = 29, Name = "Drafting and reviewing corporate tax plans", SpecializationID = 8 },
            new SubSpecialization
                { ID = 30, Name = "Registration and protection of patents and trademarks", SpecializationID = 9 },
            new SubSpecialization
                { ID = 31, Name = "Copyright and Intellectual Property Infringement Issues", SpecializationID = 9 },
            new SubSpecialization { ID = 32, Name = "Copyright Law Consultations", SpecializationID = 9 }
        );
    }
}