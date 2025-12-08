using UnityEngine;
using SmartRetailAR.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmartRetailAR.Recommendations
{
    public class FilterSystem : MonoBehaviour
    {
        [System.Serializable]
        public class FilterCriteria
        {
            public string category;
            public string subcategory;
            public float minPrice;
            public float maxPrice;
            public bool? isBio;
            public bool? isVegan;
            public bool? isVegetarian;
            public bool? isGlutenFree;
            public bool? isLactoseFree;
            public string ecoScore;
            public string healthScore;
            public string nutriScore;
            public string origin;
            public List<string> certifications;
            public string availability;

            public FilterCriteria()
            {
                minPrice = 0f;
                maxPrice = float.MaxValue;
                certifications = new List<string>();
            }
        }

        private FilterCriteria _currentFilters;

        void Start()
        {
            _currentFilters = new FilterCriteria();
        }

        public List<ProductData> ApplyFilters(List<ProductData> products, FilterCriteria filters = null)
        {
            if (filters == null)
                filters = _currentFilters;

            var filtered = products.AsEnumerable();

            // Category filter
            if (!string.IsNullOrEmpty(filters.category))
                filtered = filtered.Where(p => p.category == filters.category);

            if (!string.IsNullOrEmpty(filters.subcategory))
                filtered = filtered.Where(p => p.subcategory == filters.subcategory);

            // Price filter
            filtered = filtered.Where(p => p.price >= filters.minPrice && p.price <= filters.maxPrice);

            // Boolean flags filters
            if (filters.isBio.HasValue)
                filtered = filtered.Where(p => p.flags.isBio == filters.isBio.Value);

            if (filters.isVegan.HasValue)
                filtered = filtered.Where(p => p.flags.isVegan == filters.isVegan.Value);

            if (filters.isVegetarian.HasValue)
                filtered = filtered.Where(p => p.flags.isVegetarian == filters.isVegetarian.Value);

            if (filters.isGlutenFree.HasValue)
                filtered = filtered.Where(p => p.flags.isGlutenFree == filters.isGlutenFree.Value);

            if (filters.isLactoseFree.HasValue)
                filtered = filtered.Where(p => p.flags.isLactoseFree == filters.isLactoseFree.Value);

            // Score filters
            if (!string.IsNullOrEmpty(filters.ecoScore))
                filtered = filtered.Where(p => p.scores.ecoScore == filters.ecoScore);

            if (!string.IsNullOrEmpty(filters.healthScore))
                filtered = filtered.Where(p => p.scores.healthScore == filters.healthScore);

            if (!string.IsNullOrEmpty(filters.nutriScore))
                filtered = filtered.Where(p => p.scores.nutriScore == filters.nutriScore);

            // Origin filter
            if (!string.IsNullOrEmpty(filters.origin))
                filtered = filtered.Where(p => p.origin == filters.origin);

            // Certifications filter
            if (filters.certifications != null && filters.certifications.Count > 0)
            {
                filtered = filtered.Where(p => 
                    p.certifications != null && 
                    filters.certifications.Any(cert => p.certifications.Contains(cert))
                );
            }

            // Availability filter
            if (!string.IsNullOrEmpty(filters.availability))
                filtered = filtered.Where(p => p.availability == filters.availability);

            return filtered.ToList();
        }

        public void SetFilter(FilterCriteria filters)
        {
            _currentFilters = filters;
        }

        public FilterCriteria GetCurrentFilters()
        {
            return _currentFilters;
        }

        public void ClearFilters()
        {
            _currentFilters = new FilterCriteria();
        }

        public List<ProductData> GetFilteredProducts()
        {
            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();
            return ApplyFilters(allProducts, _currentFilters);
        }

        public List<ProductData> QuickFilter(string filterType, object value)
        {
            List<ProductData> allProducts = ProductDatabase.Instance.GetAllProducts();

            switch (filterType.ToLower())
            {
                case "bio":
                    return allProducts.Where(p => p.flags.isBio).ToList();
                
                case "vegan":
                    return allProducts.Where(p => p.flags.isVegan).ToList();
                
                case "eco_a":
                    return allProducts.Where(p => p.scores.ecoScore == "A").ToList();
                
                case "health_a":
                    return allProducts.Where(p => p.scores.healthScore == "A").ToList();
                
                case "price_low":
                    return allProducts.OrderBy(p => p.price).Take(100).ToList();
                
                case "category":
                    if (value is string category)
                        return allProducts.Where(p => p.category == category).ToList();
                    break;
                
                default:
                    return allProducts;
            }

            return allProducts;
        }

        public Dictionary<string, int> GetFilterStats(List<ProductData> products)
        {
            return new Dictionary<string, int>
            {
                ["Total"] = products.Count,
                ["Bio"] = products.Count(p => p.flags.isBio),
                ["Vegan"] = products.Count(p => p.flags.isVegan),
                ["Eco_A"] = products.Count(p => p.scores.ecoScore == "A"),
                ["Health_A"] = products.Count(p => p.scores.healthScore == "A"),
                ["InStock"] = products.Count(p => p.availability == "in_stock")
            };
        }
    }
}
