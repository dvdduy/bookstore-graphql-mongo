export type GraphQLResponse<responseKey extends string, responseType> = {
    [key in responseKey]: responseType
 }