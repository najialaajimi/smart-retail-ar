#!/usr/bin/env python3
"""
Placeholder Image Generator for Smart Retail AR
Generates placeholder images for development and testing
"""

import json
import os
from pathlib import Path

def create_simple_placeholder(product_id, category, size=512):
    """
    Create a simple placeholder image without PIL dependency
    Creates an HTML file that can be screenshot for quick placeholders
    """
    colors = {
        'Dairy': '#FFF8DC',
        'Fruits': '#FFE4B5',
        'Vegetables': '#90EE90',
        'Beverages': '#87CEEB',
        'Snacks': '#FFD700',
        'Bakery': '#F4A460',
        'Meat': '#CD5C5C',
        'Seafood': '#4682B4',
        'Frozen': '#E0FFFF',
        'Pantry': '#D2B48C'
    }
    
    color = colors.get(category, '#FFFFFF')
    
    html = f"""
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                margin: 0;
                padding: 0;
            }}
            .placeholder {{
                width: {size}px;
                height: {size}px;
                background-color: {color};
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                font-family: Arial, sans-serif;
                text-align: center;
            }}
            .product-id {{
                font-size: 32px;
                font-weight: bold;
                color: #333;
                margin-bottom: 10px;
            }}
            .category {{
                font-size: 24px;
                color: #666;
            }}
        </style>
    </head>
    <body>
        <div class="placeholder">
            <div class="product-id">{product_id}</div>
            <div class="category">{category}</div>
        </div>
    </body>
    </html>
    """
    
    return html

def generate_readme():
    """Generate README for placeholder images"""
    
    readme = """# Product Images

This directory contains product images for Smart Retail AR.

## Current Status

**⚠️ Placeholder Images**

Currently using placeholder images for development. Replace with actual product photos before production release.

## Image Specifications

- **Format:** JPG or PNG
- **Resolution:** 512x512 pixels (square)
- **Size:** < 500 KB per image
- **Background:** White or transparent (PNG)
- **Naming:** prod001.jpg, prod002.jpg, etc.

## Adding Real Images

1. Take photos of products (or use stock images)
2. Edit to square format (512x512)
3. Remove background (optional, use PNG)
4. Optimize file size (< 500 KB)
5. Name correctly (prod001.jpg, prod002.jpg, etc.)
6. Place in this directory

## Recommended Tools

- **GIMP** (free) - Image editing
- **TinyPNG** (web) - Compression
- **Remove.bg** (web) - Background removal

## Status per Product

Replace placeholders with real images:

- [ ] PROD001 - Organic Dairy Product 1
- [ ] PROD002 - Fresh Fruits Product 2
- [ ] PROD003 - Natural Vegetables Product 3
- ... (47 more)

Total: 0/50 real images

---

**Note:** These placeholders are for development only. Do not use in production.
"""
    
    return readme

def main():
    print("=" * 60)
    print("  Smart Retail AR - Placeholder Image Generator")
    print("=" * 60)
    print()
    
    # Paths
    database_path = "Assets/Resources/Data/products_database.json"
    output_dir = "Assets/Resources/Images/Products"
    
    # Create output directory
    Path(output_dir).mkdir(parents=True, exist_ok=True)
    
    # Load product database
    print("Loading product database...")
    try:
        with open(database_path, 'r', encoding='utf-8') as f:
            database = json.load(f)
        
        products = database.get('products', [])
        print(f"Found {len(products)} products")
        
        # Generate placeholder info file
        print("\nGenerating placeholder image info...")
        
        placeholders_info = []
        
        for product in products:
            product_id = product['id']
            category = product['category']
            
            # Create HTML placeholder
            html = create_simple_placeholder(product_id, category)
            html_file = f"{output_dir}/{product_id.lower()}_placeholder.html"
            
            with open(html_file, 'w', encoding='utf-8') as f:
                f.write(html)
            
            placeholders_info.append({
                'id': product_id,
                'name': product['name'],
                'category': category,
                'html_file': html_file,
                'expected_image': f"{output_dir}/prod{product_id[4:]}.jpg"
            })
            
            print(f"  ✓ Created placeholder info for {product_id}")
        
        # Generate README
        readme_file = f"{output_dir}/README.md"
        with open(readme_file, 'w', encoding='utf-8') as f:
            f.write(generate_readme())
        
        print(f"\n✅ Generated placeholder info for {len(products)} products")
        print(f"📁 Output directory: {output_dir}/")
        print("\n📝 Next steps:")
        print("   1. Open HTML files in browser to see placeholders")
        print("   2. Screenshot if needed for quick testing")
        print("   3. Replace with real product photos for production")
        print("\n💡 For real images:")
        print("   - Use 512x512 px square images")
        print("   - Name as prod001.jpg, prod002.jpg, etc.")
        print("   - Keep file size < 500 KB")
        
        # Generate master HTML with all placeholders
        generate_master_html(placeholders_info, output_dir)
        
    except FileNotFoundError:
        print(f"\n❌ Error: Database not found at {database_path}")
        print("   Make sure you're running this from the project root directory")
    except Exception as e:
        print(f"\n❌ Error: {e}")

def generate_master_html(placeholders_info, output_dir):
    """Generate a master HTML file showing all placeholders"""
    
    html_content = """
    <!DOCTYPE html>
    <html>
    <head>
        <title>Smart Retail AR - Product Image Placeholders</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                padding: 20px;
                background-color: #f5f5f5;
            }
            h1 {
                color: #333;
            }
            .grid {
                display: grid;
                grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
                gap: 20px;
                margin-top: 20px;
            }
            .item {
                background: white;
                padding: 15px;
                border-radius: 8px;
                box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                text-align: center;
            }
            .placeholder {
                width: 150px;
                height: 150px;
                margin: 0 auto 10px;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                border-radius: 4px;
            }
            .product-id {
                font-weight: bold;
                font-size: 14px;
                margin-bottom: 5px;
            }
            .product-name {
                font-size: 12px;
                color: #666;
            }
            .category {
                font-size: 10px;
                color: #999;
                margin-top: 5px;
            }
        </style>
    </head>
    <body>
        <h1>🖼️ Product Image Placeholders</h1>
        <p>These are placeholder images for development. Replace with real product photos.</p>
        <div class="grid">
    """
    
    colors = {
        'Dairy': '#FFF8DC',
        'Fruits': '#FFE4B5',
        'Vegetables': '#90EE90',
        'Beverages': '#87CEEB',
        'Snacks': '#FFD700',
        'Bakery': '#F4A460',
        'Meat': '#CD5C5C',
        'Seafood': '#4682B4',
        'Frozen': '#E0FFFF',
        'Pantry': '#D2B48C'
    }
    
    for info in placeholders_info:
        color = colors.get(info['category'], '#FFFFFF')
        html_content += f"""
            <div class="item">
                <div class="placeholder" style="background-color: {color}">
                    <div style="font-weight: bold;">{info['id']}</div>
                    <div style="font-size: 10px; margin-top: 5px;">{info['category']}</div>
                </div>
                <div class="product-id">{info['id']}</div>
                <div class="product-name">{info['name'][:30]}...</div>
                <div class="category">{info['category']}</div>
            </div>
        """
    
    html_content += """
        </div>
        <div style="margin-top: 40px; padding: 20px; background: #fff; border-radius: 8px;">
            <h2>📸 Adding Real Images</h2>
            <ol>
                <li>Take or source product photos</li>
                <li>Edit to 512x512 pixels square format</li>
                <li>Optimize to &lt; 500 KB</li>
                <li>Name as prod001.jpg, prod002.jpg, etc.</li>
                <li>Place in Assets/Resources/Images/Products/</li>
            </ol>
        </div>
    </body>
    </html>
    """
    
    master_file = f"{output_dir}/all_placeholders.html"
    with open(master_file, 'w', encoding='utf-8') as f:
        f.write(html_content)
    
    print(f"\n📄 Master placeholder view: {master_file}")
    print("   Open this file in browser to see all placeholders")

if __name__ == "__main__":
    main()
