export function formatColumns(data: any[]) {
  const columns: { field: string; name: string }[] = [];
  const fieldNames = new Set<string>();

  data.forEach((d) => {
    Object.keys(d).forEach((key) => {
      if (key !== "name" && key !== "id") {
        fieldNames.add(key);
      }
    });
  });

  fieldNames.forEach((fieldName) => {
    const formattedFieldName = fieldName.includes("_")
      ? fieldName.split("_")[0]
      : fieldName;
    columns.push({ field: fieldName, name: formattedFieldName });
  });
  
  return columns;
}