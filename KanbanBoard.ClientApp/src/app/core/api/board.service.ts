import { inject, Injectable, Signal } from '@angular/core';
import { HttpClient, httpResource } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { BoardDto, CreateUpdateBoardDto } from '@core/model/project-management.dto';

@Injectable({ providedIn: 'root' })
export class BoardService{
     
    private readonly http  = inject(HttpClient);
    protected readonly endpoint = 'http://localhost:5025/api/board';

    getBoards(params: ()=> {search: string}){
        return httpResource<BoardDto[]>(()=>{
            const {search} = params();
            return `${this.endpoint}${search ? '?q=' + search: ''}`
        })
    }

    getBoard(id: Signal<string | null>) {
        return httpResource<BoardDto>(() => {
        const boardId = id();
        return boardId ? `${this.endpoint}/${boardId}` : undefined;
        });
    }

    async create(dto: CreateUpdateBoardDto): Promise<BoardDto>{
        return firstValueFrom(
            this.http.post<BoardDto>(this.endpoint,dto)
        )
    }

    async update(id: string, dto: CreateUpdateBoardDto): Promise<BoardDto> {
        return firstValueFrom(
        this.http.put<BoardDto>(`${this.endpoint}/${id}`, dto)
        );
    }

    async delete(id: string): Promise<void> {
        return firstValueFrom(
        this.http.delete<void>(`${this.endpoint}/${id}`));
    }
}