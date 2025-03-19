import { ToDoAddDto } from "../models/ToDo/ToDoAddDto";
import { ToDoDto } from "../models/ToDo/ToDoDto";
import { ToDoGetByDateAndTimeBlockDto } from "../models/ToDo/ToDoGetByDateAndTimeBlockDto";
import { ToDoUpdateDto } from "../models/ToDo/ToDoUpdateDto";
import api from "./axiosInstance";

export const getToDoById = async (toDoId: string): Promise<ToDoDto> => {
  const response = await api.get<ToDoDto>("/${toDoId}");
  return response.data;
};

export const getToDos = async (
  toDoData: ToDoGetByDateAndTimeBlockDto
): Promise<ToDoDto[]> => {
  const response = await api.get<ToDoDto[]>("/", { params: toDoData });
  return response.data;
};

export const createToDo = async (toDoData: ToDoAddDto): Promise<ToDoDto> => {
  const response = await api.post<ToDoDto>("/", toDoData);
  return response.data;
};

export const updateToDo = async (toDoData: ToDoUpdateDto): Promise<void> => {
  await api.put("/", toDoData);
};

export const deleteToDo = async (toDoId: string): Promise<void> => {
  await api.delete("/${toDoId}");
};
