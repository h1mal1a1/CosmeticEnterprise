export type fileInfo = {
  file: File
  name: string
  size: number
  status: string
  progress?: number // 0..100
};