import React from "react";
import SortPanel from "./ServiceSorter";

interface SearchBarProps {
  searchTerm: string;
  onSearchChange: (value: string) => void;
  sortOrder: "asc" | "desc";
  onSortChange: (order: "asc" | "desc") => void;
}

const SearchBar: React.FC<SearchBarProps> = ({
  searchTerm,
  onSearchChange,
  sortOrder,
  onSortChange,
}) => {
  return (
    <div className="filter-bar">
      <input
        type="text"
        placeholder="Поиск услуг..."
        className="search-input"
        value={searchTerm}
        onChange={(e) => onSearchChange(e.target.value)}
      />
      <SortPanel sortOrder={sortOrder} onSortChange={onSortChange} />
    </div>
  );
};

export default SearchBar;
