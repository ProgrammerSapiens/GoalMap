import styles from "../styles/MainContent.module.css";

const MainContent = () => {
  return (
    <div className="flex-1">
      <table className={styles.table}>
        <thead className={styles.thead}>
          <tr>
            <th className="px-1 py-2 text-center">
              <input type="checkbox" />
            </th>
            <th className="px-32 py-2 text-center border-l border-black">
              Description
            </th>
            <th className="px-16 py-2 text-center border-l border-black">
              Category
            </th>
            <th className="py-2 text-center border-l border-black">
              Difficulty
            </th>
            <th className="px-4 py-2 text-center border-l border-black">
              Deadline
            </th>
            <th className="py-2 text-center border-l border-black">
              Repeat Frequency
            </th>
          </tr>
        </thead>
      </table>
    </div>
  );
};

export default MainContent;
