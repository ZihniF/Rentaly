using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Models;

public class HomeViewModel
{
    public List<HomeContent> Processes { get; set; } = [];
    public List<HomeContent> Futures { get; set; } = [];
    public List<HomeContent> Statistics { get; set; } = [];
    public List<HomeContent> Awards { get; set; } = [];
    public List<HomeContent> Testimonials { get; set; } = [];
    public List<HomeContent> Faqs { get; set; } = [];
    public List<Car> Cars { get; set; } = [];
    public List<Brand> Brands { get; set; } = [];
    public List<CarModel> Models { get; set; } = [];
    public List<Branch> Branches { get; set; } = [];
    public int? SelectedBranchId { get; set; }
    public DateTime? PickupDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool AvailabilitySearchApplied { get; set; }
}
