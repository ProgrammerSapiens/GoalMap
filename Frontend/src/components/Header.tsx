import { CircleUser } from "lucide-react";
import styles from "../styles/Header.module.css";

const Header = () => {
  return (
    <header className={styles.header}>
      <h1 className="text-xl">GoalMap</h1>
      <CircleUser size={32} className={styles.icon} />
    </header>
  );
};

export default Header;
