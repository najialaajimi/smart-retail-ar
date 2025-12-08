import json
import random

# Categories and subcategories
categories = {
    "Alimentation": ["Fruits & Légumes", "Viandes & Poissons", "Produits Laitiers", "Épicerie Salée", "Épicerie Sucrée", "Boissons", "Surgelés"],
    "Hygiène & Beauté": ["Soins Corps", "Soins Visage", "Hygiène Buccodentaire", "Maquillage", "Parfums"],
    "Entretien": ["Lessive", "Produits Ménagers", "Vaisselle"],
    "Bébé": ["Alimentation Bébé", "Hygiène Bébé", "Couches"],
    "Animalerie": ["Chiens", "Chats", "Oiseaux"]
}

brands = ["Bio Coop", "Carrefour Bio", "Auchan", "Leader Price", "U", "Intermarché", 
          "Leclerc", "Casino", "Monoprix", "Naturalia", "La Vie Claire", "Biocoop",
          "Danone", "Nestlé", "Coca-Cola", "Ferrero", "Unilever", "L'Oréal",
          "Kellogg's", "PepsiCo", "Mars", "Procter & Gamble", "Henkel", "Beiersdorf"]

origins = ["France", "Italie", "Espagne", "Allemagne", "Belgique", "Pays-Bas", 
           "Portugal", "Grèce", "Maroc", "Tunisie", "Brésil", "Argentine",
           "USA", "Chine", "Japon", "Thaïlande", "Inde"]

certifications = ["AB", "Bio Europe", "Label Rouge", "AOC", "AOP", "IGP", 
                 "Écocert", "Max Havelaar", "Rainforest Alliance", "MSC",
                 "Commerce Équitable", "Demeter", "Nature & Progrès"]

products = []

for i in range(5000):
    product_id = f"PROD{str(i+1).zfill(6)}"
    barcode = f"3{random.randint(100000000000, 999999999999)}"
    qr_code = f"QR{product_id}"
    
    category = random.choice(list(categories.keys()))
    subcategory = random.choice(categories[category])
    brand = random.choice(brands)
    origin = random.choice(origins)
    
    # Nutritional values
    calories = random.randint(50, 600)
    proteins = round(random.uniform(0.5, 30), 1)
    carbs = round(random.uniform(1, 80), 1)
    fats = round(random.uniform(0.1, 40), 1)
    fibers = round(random.uniform(0, 15), 1)
    salt = round(random.uniform(0, 5), 2)
    sugars = round(random.uniform(0, 50), 1)
    
    # Scores
    eco_score = random.choice(["A", "B", "C", "D", "E"])
    health_score = random.choice(["A", "B", "C", "D", "E"])
    nutri_score = random.choice(["A", "B", "C", "D", "E"])
    social_score = round(random.uniform(1, 10), 1)
    
    # Flags
    is_bio = random.random() > 0.7
    is_vegan = random.random() > 0.8
    is_vegetarian = random.random() > 0.6
    is_gluten_free = random.random() > 0.85
    is_lactose_free = random.random() > 0.85
    
    # Certifications
    product_certs = []
    if is_bio:
        product_certs.append(random.choice(["AB", "Bio Europe", "Écocert"]))
    if random.random() > 0.7:
        product_certs.append(random.choice(certifications))
    
    # Generate product name
    prefixes = ["", "Bio ", "Éco ", "Premium ", "Extra ", "Select "]
    suffixes = ["", " Light", " Sans Sucres", " Artisanal", " Traditionnel"]
    product_name = f"{random.choice(prefixes)}{subcategory} {brand}{random.choice(suffixes)}"
    
    # Price
    price = round(random.uniform(0.99, 49.99), 2)
    
    # Alternatives (random selection of other products)
    num_alternatives = random.randint(2, 5)
    
    product = {
        "id": product_id,
        "name": product_name,
        "brand": brand,
        "barcode": barcode,
        "qrCode": qr_code,
        "category": category,
        "subcategory": subcategory,
        "description": f"{product_name} - Produit de qualité {origin}",
        "origin": origin,
        "price": price,
        "currency": "EUR",
        "weight": f"{random.randint(100, 2000)}g",
        "nutritionalInfo": {
            "servingSize": "100g",
            "calories": calories,
            "proteins": proteins,
            "carbohydrates": carbs,
            "fats": fats,
            "fibers": fibers,
            "salt": salt,
            "sugars": sugars
        },
        "scores": {
            "ecoScore": eco_score,
            "healthScore": health_score,
            "nutriScore": nutri_score,
            "socialScore": social_score
        },
        "certifications": product_certs,
        "flags": {
            "isBio": is_bio,
            "isVegan": is_vegan,
            "isVegetarian": is_vegetarian,
            "isGlutenFree": is_gluten_free,
            "isLactoseFree": is_lactose_free
        },
        "imageUrl": f"products/{product_id.lower()}.png",
        "model3D": f"models/{product_id.lower()}.obj",
        "availability": random.choice(["in_stock", "low_stock", "out_of_stock"]),
        "stockQuantity": random.randint(0, 500),
        "alternatives": []
    }
    
    products.append(product)

# Add alternatives after all products are created
for product in products:
    num_alternatives = random.randint(2, 5)
    alternatives = random.sample([p["id"] for p in products if p["id"] != product["id"]], num_alternatives)
    product["alternatives"] = alternatives

# Create the database structure
database = {
    "version": "1.0.0",
    "lastUpdate": "2025-12-08",
    "totalProducts": len(products),
    "products": products
}

# Save to JSON
with open('/home/runner/work/smart-retail-ar/smart-retail-ar/Assets/Resources/Data/products_database.json', 'w', encoding='utf-8') as f:
    json.dump(database, f, ensure_ascii=False, indent=2)

print(f"Generated {len(products)} products successfully!")
