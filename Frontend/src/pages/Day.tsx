import React from "react";
import { CircleUser } from "lucide-react";

const Day: React.FC = () => {
  return (
    <div className="min-h-screen flex flex-col">
      {/* Header */}
      <header className="bg-[#8E8D8A] text-[#EAE7DC] p-4 flex flex-row justify-between">
        <h1 className="text-xl">GoalMap</h1>
        <CircleUser size={32} className="cursor-pointer hover:text-gray-300" />
      </header>

      {/*Content area*/}
      <div className="flex flex-1">
        {/* Sidebar */}
        <aside className="bg-[#E98074] w-64 p-4 flex flex-col justify-between">
          <nav className="flex flex-col space-y-2">
            <button className="bg-[#333] hover:bg-[#555] text-white p-3 rounded-md">
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

        {/* Central content area */}
        <main className="flex-1 flex flex-col bg-[#EAE7DC] text-[#E85A4F]">
          {/* Instruments panel */}
          <div className="bg-[#D8C3A5] text-[#EAE7DC] p-4"></div>

          {/* Main content */}
          <div className="flex-1 p-2">
            <h2 className="text-lg text-center">Main content</h2>
            <p>Table with tasks.</p>
          </div>
        </main>
      </div>

      {/* Footer */}
      <footer className="bg-[#8E8D8A] text-[#EAE7DC] text-center p-4">
        <p>&copy; 2025 GoalMap</p>
      </footer>
    </div>
  );
};

export default Day;
