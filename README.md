# Instruções para conseguir rodar o projeto e testa-lo 


# Para testes, obrigatório ter uma máquina virtual, porque para testes o servidor precisa de 2 máquinas conectadas a ele.

# 1 - Ter uma máquina virtual com Windows (a partir do Windows 10 pra cima)
# 2 - Instalar o Visual Studio 2022 Community Edition
# 3 - Clonar o repositório na máquina virtual

# Rodando o projeto:

# 1 - Quando o projeto for iniciado (provavelmente ele vai abrir os dois, client e server) na maquina virtual...
# não a necessidade de inicializar o servidor, pois ele já vai estar rodando na maquina local.

# 2 - Os ip's do servidor que estão funcionando para o cliente se conectar ao servidor são ip's nesse padrão...
# 26.24.181.221
# 192.168.56.1
# 192.168.22.43
# A porta em seguida do ip está configurada para ser 11000, a forma de digitar o ip para se conectar...
# segue esse padrão 192.168.56.1:11000

# O problema que está tendo nesse projeto é que quando o player 2 se conectava, o servidor não esperava...
# ele inserir o Nickname e já iniciava a UI.