import csv
import os

'''Parse csv file to consts'''

def wrap_quotes_if_string(value, data_type):
    if data_type == 'string':
        return f'"{value}"'
    elif data_type == 'float':
        return f"{value}f"
    elif data_type == 'int[]':
        return f"new int[]{{{value}}}"
    elif data_type == 'string[]':
        return f"new string[]{{{value}}}"
    else:
        return value

def convert_csv_to_cs(csv_filename, output_filename):
    with open(csv_filename, 'r') as f:
        reader = csv.reader(f)
        lines = list(reader)
        dataHeader = lines[0]
        dataType = lines[1]
        lines = lines[2:]

        className = os.path.basename(csv_filename).replace('.csv', 'Data')
        structName = className + 'Struct'
        with open(output_filename, 'w') as outfile:
            outfile.write('using System.Collections.Generic;\n\n')
            outfile.write(f'public static class {className}\n')
            outfile.write('{\n')
            # Define MyData class
            outfile.write(f'    public class {structName}\n')
            outfile.write('    {\n')
            for h, t in zip(dataHeader, dataType):
                outfile.write(f'        public {t} {h};\n')
            outfile.write('\n')
        
            # Add constructor
            constructor_args = ', '.join([f"{t} {h.lower()}" for h, t in zip(dataHeader, dataType)])
            constructor_body = '\n'.join([f"            this.{h} = {h.lower()};" for h in dataHeader])
            outfile.write(f'        public {structName}({constructor_args})\n')
            outfile.write('        {\n')
            outfile.write(constructor_body)
            outfile.write('\n        }\n')
            outfile.write('    }\n')
        
            # Create dictionary
            outfile.write(f'    public static Dictionary<int, {structName}> data = new Dictionary<int, {structName}>\n')
            outfile.write('    {\n')
        
            for line in lines:
                values = [wrap_quotes_if_string(value, t) for value, t in zip(line, dataType)]
                id_value = values[0]
                other_values = ', '.join(values[1:])
                outfile.write(f'        {{{id_value}, new {structName}({id_value}, {other_values})}},\n')
            outfile.write('    };\n')

            # Add a getter
            outfile.write(f"""
    public static {structName} GetData(int id)
    {{
        if (data.TryGetValue(id, out {structName} result))
        {{
            return result;
        }}
        else
        {{
            return null;
        }}
    }}
""")
            outfile.write('}\n')

    print(f"Finished converting {csv_filename} to {output_filename}")

root_dir = os.path.join(os.path.dirname(__file__), '..', '..')

csv_folder = os.path.join(root_dir, 'Designs', 'General')
output_folder = os.path.join(root_dir, 'Scripts', 'Contents', 'General')

# Ensure output folder exists
if not os.path.exists(output_folder):
    os.makedirs(output_folder)

# Loop through each csv file in the directory
for filename in os.listdir(csv_folder):
    if filename.endswith('.csv'):
        csv_filename = os.path.join(csv_folder, filename)
        output_filename = os.path.join(output_folder, filename.replace('.csv', 'Data.cs'))
        convert_csv_to_cs(csv_filename, output_filename)

# Additional code here if needed
print("All csv files have been converted.")