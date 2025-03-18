import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Day from "./pages/Day";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Day />} />
      </Routes>
    </Router>
  );
}

export default App;
