import React from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import InstrumentsPanel from "../components/InstrumentsPanel";
import MainContent from "../components/MainContent";
import Footer from "../components/Footer";

const Day: React.FC = () => {
  return (
    <div className="min-h-screen flex flex-col">
      <Header />

      {/*Content area*/}
      <div className="flex flex-1">
        <Sidebar />

        {/* Central content area */}
        <main className="flex-1 flex flex-col bg-[#EAE7DC] text-[#E85A4F]">
          <InstrumentsPanel />
          <MainContent />
        </main>
      </div>
      
      <Footer />
    </div>
  );
};

export default Day;
