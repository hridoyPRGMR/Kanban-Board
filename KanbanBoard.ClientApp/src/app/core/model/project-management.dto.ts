export interface CreateUpdateProjectDto
{
    name:string;
    description?:string;
}

export interface ProjectDto{
    id: string;
    name: string;
    description?: string;
    createdAt: Date;
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