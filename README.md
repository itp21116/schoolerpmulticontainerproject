# schoolerpmulticontainerproject
schoolerpmulticontainerproject with kubernetes
kubectl create secret generic mssql-secret --from-literal=SA_PASSWORD="2Secure*Password2"
kubectl apply -f sql.yaml
kubectl apply -f school.yaml
kubectl apply -f students.yaml
kubectl apply -f courses.yaml
Αφού βεβαιωθούμε πως είναι όλα running (kubectl get all) ανοίγουμε έναν browser στην http://localhost:30000

