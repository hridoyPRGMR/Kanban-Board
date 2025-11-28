export interface Task {
  id: string;
  title: string;
  tag: string;
  assignee: string; // Added assignee for card detail
  dueDate: string;  // Added dueDate for card detail
}

export interface Column {
  id: string;
  title: string;
  tasks: Task[];
  color: string; // Added color for visual identification
}

export const INITIAL_COLUMNS: Column[] = [
  {
    id: '1', title: 'To Do', color: 'blue',
    tasks: [
      { id: 't1', title: 'Design System Audit', tag: 'Design', assignee: 'Alex', dueDate: 'Nov 28' },
      { id: 't2', title: 'Setup Repo & CI/CD', tag: 'DevOps', assignee: 'Jane', dueDate: 'Dec 1' },
      { id: 't3', title: 'Implement Login Flow', tag: 'Frontend', assignee: 'Alex', dueDate: 'Dec 5' }
    ]
  },
  {
    id: '2', title: 'In Progress', color: 'indigo',
    tasks: [
      { id: 't4', title: 'Build Kanban Layout', tag: 'Frontend', assignee: 'Sarah', dueDate: 'Today' },
      { id: 't5', title: 'Develop API Endpoints', tag: 'Backend', assignee: 'Mike', dueDate: 'Dec 10' }
    ]
  },
  {
    id: '3', title: 'Review', color: 'yellow',
    tasks: [
      { id: 't6', title: 'Write & Run Unit Tests', tag: 'QA', assignee: 'Jane', dueDate: 'Dec 2' }
    ]
  },
  {
    id: '4', title: 'Done', color: 'green',
    tasks: []
  },
];