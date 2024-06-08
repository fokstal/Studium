export function objectToFormData(obj: Object) {
  const formData = new FormData();

  Object.entries(obj).forEach(([key, value]) => {
    formData.append(key, value as Blob);
  });

  return formData;
}