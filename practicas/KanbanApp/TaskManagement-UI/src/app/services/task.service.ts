import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { TaskDto} from "../models/task.model";
import { CreateTaskRequest } from "../models/task.model";

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'https://localhost:5282/api/tasks';

constructor(private client: HttpClient) {}

  getTasks(): Observable<TaskDto[]> {
    return this.client.get<TaskDto[]>(this.apiUrl);
  }

  createTask(request: CreateTaskRequest): Observable<TaskDto> {
    return this.client.post<TaskDto>(this.apiUrl, request);
  }

  completeTask(id: string): Observable<void> {
    return this.client.patch<void>(`${this.apiUrl}/${id}/complete`, {});
  }

}