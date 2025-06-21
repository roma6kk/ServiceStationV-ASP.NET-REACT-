import React from "react";
import "../styles/ServiceSorter.css"
type SortOrder = "asc" | "desc";

interface SortPanelProps {
  sortOrder: SortOrder;
  onSortChange: (order: SortOrder) => void;
}

function SortPanel({ sortOrder, onSortChange }: SortPanelProps) {
  return (
    <div className="sort-panel">
      <select
        value={sortOrder}
        onChange={(e) => onSortChange(e.target.value as SortOrder)}
        className="sort-select"
      >
        <option value="asc">Price by ASC</option>
        <option value="desc">Price by DESC</option>
      </select>
    </div>
  );
}

export default SortPanel;