#!/usr/bin/env python3
"""
QR Code Generator for Smart Retail AR
Generates QR codes for all products in the database
"""

import json
import qrcode
import os
from pathlib import Path

def generate_qr_codes():
    """Generate QR codes for all products in the database"""
    
    # Paths
    database_path = "Assets/Resources/Data/products_database.json"
    output_dir = "Assets/Resources/QRCodes"
    
    # Create output directory if it doesn't exist
    Path(output_dir).mkdir(parents=True, exist_ok=True)
    
    # Load product database
    print("Loading product database...")
    with open(database_path, 'r', encoding='utf-8') as f:
        database = json.load(f)
    
    products = database.get('products', [])
    print(f"Found {len(products)} products")
    
    # Generate QR codes
    print("\nGenerating QR codes...")
    generated_count = 0
    
    for product in products:
        product_id = product['id']
        qr_code_data = product.get('qrCode', product_id)
        
        # Create QR code
        qr = qrcode.QRCode(
            version=1,
            error_correction=qrcode.constants.ERROR_CORRECT_L,
            box_size=10,
            border=4,
        )
        qr.add_data(qr_code_data)
        qr.make(fit=True)
        
        # Create image
        img = qr.make_image(fill_color="black", back_color="white")
        
        # Save image
        filename = f"{output_dir}/{product_id}.png"
        img.save(filename)
        
        generated_count += 1
        print(f"  ✓ Generated QR code for {product_id}: {product['name']}")
    
    print(f"\n✅ Successfully generated {generated_count} QR codes")
    print(f"📁 Saved to: {output_dir}/")
    
    # Generate printable sheet
    generate_printable_sheet(products, output_dir)

def generate_printable_sheet(products, output_dir):
    """Generate a printable HTML sheet with all QR codes"""
    
    html_content = """
    <!DOCTYPE html>
    <html>
    <head>
        <title>Smart Retail AR - QR Codes</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                padding: 20px;
            }
            .qr-grid {
                display: grid;
                grid-template-columns: repeat(4, 1fr);
                gap: 20px;
                page-break-inside: avoid;
            }
            .qr-item {
                border: 2px solid #333;
                padding: 10px;
                text-align: center;
                break-inside: avoid;
            }
            .qr-item img {
                width: 150px;
                height: 150px;
                margin: 10px 0;
            }
            .qr-id {
                font-weight: bold;
                font-size: 14px;
                margin-bottom: 5px;
            }
            .qr-name {
                font-size: 12px;
                color: #666;
            }
            @media print {
                .qr-grid {
                    grid-template-columns: repeat(3, 1fr);
                }
            }
        </style>
    </head>
    <body>
        <h1>Smart Retail AR - QR Codes</h1>
        <p>Scan these QR codes with the Smart Retail AR app to view product information.</p>
        <div class="qr-grid">
    """
    
    for product in products:
        product_id = product['id']
        product_name = product['name']
        qr_image = f"{product_id}.png"
        
        html_content += f"""
            <div class="qr-item">
                <div class="qr-id">{product_id}</div>
                <img src="{qr_image}" alt="{product_id}">
                <div class="qr-name">{product_name}</div>
            </div>
        """
    
    html_content += """
        </div>
    </body>
    </html>
    """
    
    # Save HTML file
    html_file = f"{output_dir}/printable_qr_codes.html"
    with open(html_file, 'w', encoding='utf-8') as f:
        f.write(html_content)
    
    print(f"\n📄 Printable QR code sheet generated: {html_file}")
    print("   Open this file in a browser to print all QR codes")

def main():
    print("=" * 60)
    print("  Smart Retail AR - QR Code Generator")
    print("=" * 60)
    print()
    
    try:
        generate_qr_codes()
        print("\n✨ QR code generation completed successfully!")
        print("\nNext steps:")
        print("  1. Open Assets/Resources/QRCodes/printable_qr_codes.html")
        print("  2. Print the QR codes (recommended: 3x3 cm minimum)")
        print("  3. Apply QR codes to physical products or display them")
        print("  4. Test scanning with the Smart Retail AR app")
    except FileNotFoundError as e:
        print(f"\n❌ Error: {e}")
        print("   Make sure you're running this script from the project root directory")
    except Exception as e:
        print(f"\n❌ Unexpected error: {e}")

if __name__ == "__main__":
    main()
