// project.service.ts
import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { CreateUpdateProjectDto, ProjectDto } from "../model/project-management.dto";

@Injectable({ providedIn: 'root' })
export class ProjectService extends BaseService<ProjectDto,CreateUpdateProjectDto> {

  protected readonly endpoint = 'http://localhost:5025/api/project';

  constructor() {
    super();
  }

  archiveProject(id: string) {
    return this.http.patch(`${this.endpoint}/${id}/archive`, {});
  }
}