export function calculateAge(birthDate: string | Date) {
  const ageDifMs = Date.now() - +new Date(birthDate);
  const ageDate = new Date(ageDifMs);
  return Math.abs(ageDate.getUTCFullYear() - 1970);
}