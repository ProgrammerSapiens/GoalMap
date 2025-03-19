export interface ToDoUpdateDto {
  toDoId: string;
  description: string;
  difficulty: number;
  deadline: string;
  toDoDate: string;
  completionStatus: boolean;
  repeatFrequency: number;
  toDoCategoryId: string;
}
