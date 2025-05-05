export interface SimplifiedTask {
  id: number,
  taskTitle: string;
  taskDescription: string;
  taskDueDate: string;
}

export interface GroupedTasks {
    taskStatus: string;
    tasks: SimplifiedTask[];
}