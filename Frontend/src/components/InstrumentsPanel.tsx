import { List, Calendar, Plus } from "lucide-react";

const InstrumentsPanel = () => {
  return (
    <div className="bg-[#D8C3A5] text-[#EAE7DC] p-4 flex items-center justify-between">
      <div className="flex items-center gap-2">
        <List className="w-5 h-5" />
        <select className="bg-white text-[#333] p-2 rounded-md border border-gray-300">
          <option>Default</option>
          <option>Difficulty</option>
          <option>Category</option>
        </select>
      </div>

      <div className="flex items-center gap-2">
        <Calendar className="w-5 h-5" />
        <input
          type="date"
          className="bg-white text-[#333] p-2 rounded-md border border-gray-300"
        />
      </div>

      <button className="bg-[#EAE7DC] hover:bg-[#B5A38D] text-[#333] p-2 rounded-md flex items-center gap-2">
        <Plus className="w-5 h-5" />
      </button>
    </div>
  );
};

export default InstrumentsPanel;
