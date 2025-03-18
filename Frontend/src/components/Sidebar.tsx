const Sidebar = () => {
  return (
    <aside className="bg-[#E98074] w-64 p-4 flex flex-col justify-between">
      <nav className="flex flex-col space-y-2">
        <button className="bg-[#D8C3A5] text-[#EAE7DC] p-3 rounded-md">
          Day
        </button>
        <button className="bg-[#333] hover:bg-[#555] text-white p-3 rounded-md">
          Week
        </button>
        <button className="bg-[#333] hover:bg-[#555] text-white p-3 rounded-md">
          Month
        </button>
        <button className="bg-[#333] hover:bg-[#555] text-white p-3 rounded-md">
          Year
        </button>
      </nav>

      <button className="bg-[#333] hover:bg-[#555] text-white p-3 rounded-md">
        Categories
      </button>
    </aside>
  );
};

export default Sidebar;
