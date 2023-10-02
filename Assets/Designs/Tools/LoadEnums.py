import csv
import os

'''Parse csv file to enums'''
root_dir = os.path.join(os.path.dirname(__file__), '..', '..')  
csv_filename = os.path.join(root_dir, 'Designs', 'Enums.csv')
output_filename = os.path.join(root_dir, 'Scripts', 'Contents', 'Enums.cs')

with open(csv_filename, 'r') as f:
    try:
        reader = csv.reader(f)
    except:
        print("Error to open file")
    next(reader)  # Skip the header row

    lines = sorted(list(reader), key=lambda x: x[1])  

    with open(output_filename, 'w') as outfile:
        outfile.write('public static class Enums\n')
        outfile.write('{\n')
        current_category = None
        line_count = 1

        for line in lines:
            line_count += 1

            try:
                name, category, description = line  # Removed value
            except:
                print(f"Error: Unable to process line {line} on line {line_count}")
                continue

            if category != current_category:
                if current_category is not None:
                    outfile.write('    }\n\n')  # Added spaces for indentation
                current_category = category
                outfile.write(f'    public enum {category}\n')  # Added spaces for indentation
                outfile.write('    {\n')  # Added spaces for indentation

            outfile.write(f'        {name},  // {description}\n')  # Added spaces for indentation

        outfile.write('    }\n')  # Added spaces for indentation
        outfile.write('}\n')

print("Finished")
