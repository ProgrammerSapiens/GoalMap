import { CircleUser } from "lucide-react";

const Header = () => {
  return (
    <header className="bg-[#8E8D8A] text-[#EAE7DC] p-4 flex flex-row justify-between">
      <h1 className="text-xl">GoalMap</h1>
      <CircleUser size={32} className="cursor-pointer hover:text-gray-300" />
    </header>
  );
};

export default Header;
