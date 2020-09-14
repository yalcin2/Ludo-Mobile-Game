import { Pipe, PipeTransform } from '@angular/core';

@Pipe
({
    name:'convertToSpaces'
})

export class ConvertToSpacesPipe 
                        implements PipeTransform{
    transform(value: string, character: string): string
    {
        value=value.replace(character," ");
        return value;
    }
}
