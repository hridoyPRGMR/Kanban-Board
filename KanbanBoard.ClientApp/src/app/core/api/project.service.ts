// project.service.ts
import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { CreateUpdateProjectDto, ProjectDto } from "../model/project-management.dto";
import { PaginatedResponse } from "@core/model/common.dto";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ProjectService extends BaseService<ProjectDto,CreateUpdateProjectDto> {

  protected readonly endpoint = 'http://localhost:5025/api/project';

  constructor() {
    super();
  }

  archiveProject(id: string) {
    return this.http.patch(`${this.endpoint}/${id}/archive`, {});
  }

  getProjects(searchTerm: string = '', pageNumber: number = 1, pageSize: number = 10, isDescending: boolean = false): Observable<PaginatedResponse<ProjectDto>> {
    return this.http.get<PaginatedResponse<ProjectDto>>(this.endpoint, {
      params: { searchTerm, pageNumber, pageSize, isDescending}
    });
  }
}