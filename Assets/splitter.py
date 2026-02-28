import os
from PIL import Image

def crop_three_parts(
    input_folder,
    output_folder,
    x1, y1,
    x2, y2,   # top width, height
    x3, y3,   # middle width, height
    x4, y4    # bottom width, height
):
    os.makedirs(output_folder, exist_ok=True)

    image_files = [
        f for f in os.listdir(input_folder)
        if f.lower().endswith((".png", ".jpg", ".jpeg", ".bmp", ".tiff"))
    ]

    for idx, filename in enumerate(image_files, start=1):
        image_path = os.path.join(input_folder, filename)

        with Image.open(image_path) as img:
            # --- TOP ---
            top_box = (
                x1,
                y1,
                x1 + x2,
                y1 + y2
            )
            top_crop = img.crop(top_box)

            # --- MIDDLE ---
            middle_box = (
                x1,
                y1 + y2,
                x1 + x3,
                y1 + y2 + y3
            )
            middle_crop = img.crop(middle_box)

            # --- BOTTOM ---
            bottom_box = (
                x1,
                y1 + y2 + y3,
                x1 + x4,
                y1 + y2 + y3 + y4
            )
            bottom_crop = img.crop(bottom_box)

            base_name, ext = os.path.splitext(filename)

            top_crop.save(os.path.join(output_folder, f"top_{idx}{ext}"))
            middle_crop.save(os.path.join(output_folder, f"middle_{idx}{ext}"))
            bottom_crop.save(os.path.join(output_folder, f"bottom_{idx}{ext}"))

        print(f"Processed: {filename}")

    print("All images processed successfully.")


# -------------------------
# Example usage
# -------------------------
if __name__ == "__main__":
    crop_three_parts(
        input_folder="full_sprites",
        output_folder="chopped_sprites",
        x1=32,
        y1=32,
        x2=128, y2=80,
        x3=128, y3=48,
        x4=128, y4=64
    )
    crop_three_parts(
        input_folder="full_weakspots",
        output_folder="chopped_weakspots",
        x1=32,
        y1=32,
        x2=128, y2=80,
        x3=128, y3=48,
        x4=128, y4=64
    )