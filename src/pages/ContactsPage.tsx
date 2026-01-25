export default function ContactsPage() {
  const values = [1,2,3,4,5,6]
  return (
  <div>Страница контакты
    <select>{values.map(s=> (<option key={s} value={s}>{s}</option>))}</select>
  </div>);
}