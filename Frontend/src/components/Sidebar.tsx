import styles from "../styles/Sidebar.module.css";

const Sidebar = () => {
  return (
    <aside className={styles.sidebar}>
      <nav className="flex flex-col space-y-2">
        <button className={styles.navButton}> Day </button>
        <button className={styles.navButtonAlt}>Week</button>
        <button className={styles.navButtonAlt}>Month</button>
        <button className={styles.navButtonAlt}>Year</button>
      </nav>

      <button className={styles.navButtonAlt}>Categories</button>
    </aside>
  );
};

export default Sidebar;
