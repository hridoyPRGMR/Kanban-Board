import { PagedAndSortedResultRequestDto } from "./common.dto";

export interface CreateUpdateProjectDto
{
    name:string;
    description?:string;
}

export interface ProjectDto{
    id: string;
    name: string;
    description?: string;
    createdAt: string;
}


export interface CreateUpdateBoardDto{
    name:string;
    description?:string;
}

export interface BoardDto{
    id: string;
    name:string;
    description?:string;
}

export interface ProjectFilterRequestDto extends PagedAndSortedResultRequestDto{}