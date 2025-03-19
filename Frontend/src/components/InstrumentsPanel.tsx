import { List, Calendar, Plus } from "lucide-react";
import styles from "../styles/InstrumentsPanel.module.css";

const InstrumentsPanel = () => {
  return (
    <div className={styles.panel}>
      <div className="flex items-center gap-2">
        <List className="w-5 h-5" />
        <select className={styles.selectInput}>
          <option>Default</option>
          <option>Difficulty</option>
          <option>Category</option>
        </select>
      </div>

      <div className="flex items-center gap-2">
        <Calendar className="w-5 h-5" />
        <input type="date" className={styles.selectInput} />
      </div>

      <button className={styles.addButton}>
        <Plus className="w-5 h-5" />
      </button>
    </div>
  );
};

export default InstrumentsPanel;
