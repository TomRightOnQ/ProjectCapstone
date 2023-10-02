import csv
import os

'''Parse csv file to consts'''
root_dir = os.path.join(os.path.dirname(__file__), '..', '..')  # This gets the parent directory of the script
csv_filename = os.path.join(root_dir, 'Designs', 'Prefabs.csv')
output_filename = os.path.join(root_dir, 'Scripts', 'Contents', 'PrefabData.cs')

with open(csv_filename, 'r') as f:
    try:
        reader = csv.reader(f)
    except:
        print("Error to open file")
    next(reader)  # Skip the header row

    lines = sorted(list(reader), key=lambda x: x[2])  # Sort by category

    with open(output_filename, 'w') as outfile:
        outfile.write('using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;')
        outfile.write(f'/// <summary>')
        outfile.write(f'/// Data: PrefabReferences')
        outfile.write(f'/// </summary>')
        outfile.write('public static class PrefabData\n')
        outfile.write('{\n')
        current_category = None

        line_count = 1

        for line in lines:
            line_count += 1

            try:
                name, type, category, value, description = line
            except:
                print("Error: Unable to process line", line, "on line", line_count)
                continue

            if category != current_category:
                if current_category is not None:
                    outfile.write('\n')
                current_category = category
                outfile.write(f'    // {category}\n')

            if type == "int":
                outfile.write(f'    public const int {name} = {value};  // {description}\n')
            elif type == "string":
                outfile.write(f'    public const string {name} = "{value}";  // {description}\n')
            else:
                print("Error: Unable to process type", type, "on line", line_count)
        outfile.write('}\n')
print("Finished")